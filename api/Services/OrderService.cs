using APIPractice.Enums;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using AutoMapper;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace APIPractice.Services
{
    public class OrderService : IOrderService
    {

        private readonly IMapper mapper;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderStatusRepository orderStatusRepository;
        private readonly IProductRepository<Product> productRepository;

        public OrderService(IMapper mapper, IOrderItemRepository orderItemRepository
            , ICustomerRepository customerRepository, IOrderRepository orderRepository,
            IOrderStatusRepository orderStatusRepository, IProductRepository<Product> productRepository)
        {
            this.mapper = mapper;
            this.orderItemRepository = orderItemRepository;
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
            this.orderStatusRepository = orderStatusRepository;
            this.productRepository = productRepository;
        }
        public async Task CheckOut(PurchaseOrderRequest purchaseOrders, ClaimsIdentity identity)
        {
            var userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value); 
            Guid orderId = Guid.NewGuid();
            decimal orderAmount = 0;
            List<OrderItem> orderItemList = new List<OrderItem>();

            foreach (var purchaseOrder in purchaseOrders.Items)
            {
                var orderItem = mapper.Map<OrderItem>(purchaseOrder);
                orderItem.OrderId = orderId;
                Product product= await productRepository.GetAsync(purchaseOrder.ProductId);
                if(product.Quantity < purchaseOrder.Quantity)
                {
                    throw new Exception("The Order is out of stock");
                }
                else
                {
                    product.Quantity = product.Quantity - purchaseOrder.Quantity;
                    await productRepository.UpdateQuantityAsync(purchaseOrder.ProductId, product);
                    orderItem.Product = product;
                    orderItemList.Add(orderItem);
                    orderAmount += (purchaseOrder.UnitPrice * purchaseOrder.Quantity) + 46;
                }
                        
            }

            var StatusId = await orderStatusRepository.GetIdOfStatus("billed");

            var order = new Order
            {
                Id = orderId,
                CustomerId = userId,
                Amount = orderAmount,
                OrderStatusId = StatusId,
                OrderStatus = await orderStatusRepository.GetOrderStatusById(StatusId),
                CreatedAt = DateTime.UtcNow,
                DeliveredAt = null,
                Customer = await customerRepository.GetById(userId),
                OrderItems = new List<OrderItem>()
            };
            await orderRepository.AddAsync(order);
            await orderItemRepository.AddRangeAsync(orderItemList);
            order.OrderItems = orderItemList;
            await orderRepository.UpdateAsync(orderId,order);
        }

        public async Task<List<OrderHistoryDto>> ViewHistory(ClaimsIdentity identity)
        {
            var userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<Order> orders = await orderRepository.GetOrderHistoryOfCustomer(userId);
            List<OrderHistoryDto> history = new List<OrderHistoryDto>();
            foreach (Order order in orders)
            {
                history.Add(new OrderHistoryDto { Id = order.Id, Status = order.OrderStatus.Name, 
                    Name =order.Customer.Name, Mobile=order.Customer.Phone , Address = order.Customer.Address,CreatedAt=order.CreatedAt,
                    DeliveredAt=order.DeliveredAt, Items = AddOrderItems(order), TotalItems = order.OrderItems.Count,
                    Billing = new BillingDto { ItemTotal = order.Amount - 46, DeliveryFee = 40, PlatformFee = 6
                    ,TotalBill=order.Amount}
                });
            }
            return history;

        }

        public async Task<OrderHistoryDto> ViewOrderById(Guid orderId, ClaimsIdentity identity)
        {
            try
            {
                Guid userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                var order = await orderRepository.GetOrderByIdAsync(orderId, userId);
                OrderHistoryDto history = new OrderHistoryDto
                {
                    Id = order.Id,
                    Status = order.OrderStatus.Name,
                    Name = order.Customer.Name,
                    Mobile = order.Customer.Phone,
                    Address = order.Customer.Address,
                    CreatedAt = order.CreatedAt,
                    DeliveredAt = order.DeliveredAt,
                    Items = AddOrderItems(order),
                    TotalItems = order.OrderItems.Count,
                    Billing = new BillingDto
                    {
                        ItemTotal = order.Amount - 46,
                        DeliveryFee = 40,
                        PlatformFee = 6,
                        TotalBill = order.Amount
                    }
                };
                return history;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PurchaseOrderRequest>> GetBilledOrdersAsync()
        {
            var orders = await orderRepository.GetOrdersByStatusAsync("Billed");
            return mapper.Map<List<PurchaseOrderRequest>>(orders);
        }

        public async Task<List<PurchaseOrderRequest>> GetDeliveredOrdersByEmployeeAsync(ClaimsIdentity user)
        {
            var employeeId = Guid.Parse(user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var orders = await orderRepository.GetDeliveredOrdersByEmployeeAsync(employeeId);
            return mapper.Map<List<PurchaseOrderRequest>>(orders);
        }

        // Helper Functions
        private ICollection<OrderResponseDto> AddOrderItems(Order order)
        {
            List<OrderResponseDto> OrderItems = new List<OrderResponseDto>();
            foreach (OrderItem item in order.OrderItems)
            {
                OrderItems.Add(new OrderResponseDto
                {
                    Name = item.Product.Name,
                    ImageUrl = item.Product.ImageUrl,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    UnitPrice = item.Product.Price
                });
            }
            return OrderItems;
        }
    }
}

//