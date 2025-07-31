using APIPractice.Enums;
using APIPractice.Exceptions;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using AutoMapper;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Cryptography;

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
        public async Task<List<ItemResponseDto>> CheckOut(PurchaseOrderRequest purchaseOrders, Guid userId)
        {
            Guid orderId = Guid.NewGuid();
            decimal orderAmount = 0;
            List<OrderItem> orderItemList = new List<OrderItem>();
            List<Guid> productIdList = purchaseOrders.Items.Select(p=> p.ProductId).ToList();
            List<Product> productList = await productRepository.GetAllByIdsAsync(productIdList);
            List<ItemResponseDto> ItemStatus = new List<ItemResponseDto>();   
            var productDict = productList.ToDictionary(p => p.Id);

            if (purchaseOrders == null || purchaseOrders.Items.Count() == 0)
            {
                throw new BadRequestException("No Order recieved");
            }

            foreach (var purchaseOrder in purchaseOrders.Items)
            {
                var orderItem = mapper.Map<OrderItem>(purchaseOrder);
                orderItem.OrderId = orderId;

                if (!productDict.TryGetValue(purchaseOrder.ProductId, out var product))
                {
                    ItemStatus.Add(new ItemResponseDto { status = "failed", error = "Product was Not found", 
                    productId=purchaseOrder.ProductId});
                }

                if (product.Quantity < purchaseOrder.Quantity)
                {
                    ItemStatus.Add(new ItemResponseDto
                    {
                        status = "failed",
                        error = "The Order is out of stock",
                        productId = purchaseOrder.ProductId
                    });
                }
                else
                {
                    product.Quantity = product.Quantity - purchaseOrder.Quantity;
                    orderItem.Product = product;
                    orderItemList.Add(orderItem);
                    orderAmount += purchaseOrder.UnitPrice * purchaseOrder.Quantity;

                    ItemStatus.Add(new ItemResponseDto
                    {
                        status = "success",
                        productId = purchaseOrder.ProductId
                    });
                }        
            }
            await productRepository.UpdateAllQuantityAsync(productDict);

            var orderStatus = await orderStatusRepository.GetStatus("billed");

            var order = new Order
            {
                Id = orderId,
                CustomerId = userId,
                Amount = orderAmount + 46,
                OrderStatusId = orderStatus.Id,
                OrderStatus = orderStatus,
                CreatedAt = DateTime.Now,
                DeliveredAt = null,
                Customer = await customerRepository.GetById(userId),
                OrderItems = orderItemList
            };
            await orderRepository.AddAsync(order);
            return ItemStatus;
        }

        public async Task<List<OrderHistoryDto>> ViewHistory(Guid userId)
        {
            List<Order> orders = await orderRepository.GetOrderHistoryOfCustomer(userId);
            List<OrderHistoryDto> history = new List<OrderHistoryDto>();
            if (orders.Count == 0)
            {
                return history;
            }
            foreach (Order order in orders)
            {
                history.Add(new OrderHistoryDto { Id = order.Id, Status = order.OrderStatus.Name, 
                    Name =order.Customer.Name, Mobile=order.Customer.Phone , Address = order.Customer.Address,CreatedAt=order.CreatedAt,
                    DeliveredAt=order.DeliveredAt, Items = mapper.Map<List<OrderResponseDto>>(order.OrderItems.Where(u => u.OrderId == order.Id).Select(u => u.Product)), TotalItems = order.OrderItems.Count,
                    Billing = new BillingDto { ItemTotal = order.Amount - 46, DeliveryFee = 40, PlatformFee = 6
                    ,TotalBill=order.Amount}
                });
            }
            return history;

        }

        public async Task<OrderHistoryDto?> ViewOrderById(Guid orderId, Guid userId)
        {
            try
            {
                var order = await orderRepository.GetOrderByIdAsync(orderId, userId);
                if (order == null)
                {
                    return null;
                }
                OrderHistoryDto history = new OrderHistoryDto
                {
                    Id = order.Id,
                    Status = order.OrderStatus.Name,
                    Name = order.Customer.Name,
                    Mobile = order.Customer.Phone,
                    Address = order.Customer.Address,
                    CreatedAt = order.CreatedAt,
                    DeliveredAt = order.DeliveredAt,
                    Items = mapper.Map<List<OrderResponseDto>>(order.OrderItems.Where(u => u.OrderId == order.Id).Select(u => u.Product)),
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

        
    }
}

//