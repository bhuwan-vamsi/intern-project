using APIPractice.Enums;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using AutoMapper;
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
        public async Task CheckOut(OrderDto purchaseOrders, ClaimsIdentity identity)
        {
            try
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
                        orderAmount += purchaseOrder.UnitPrice * purchaseOrder.Quantity;
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
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrderHistoryDto>> ViewHistory(ClaimsIdentity identity)
        {
            try
            {
                var userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                List<Order> orders = await orderRepository.GetOrderHistoryOfCustomer(userId);
                List<OrderHistoryDto> history = new List<OrderHistoryDto>();
                foreach (Order order in orders)
                {
                    history.Add(new OrderHistoryDto { Id = order.Id, Amount=order.Amount, CreatedAt=order.CreatedAt, OrderItems = order.OrderItems,
                    Status = order.OrderStatus.Name});
                }
                return history;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

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
                    Amount = order.Amount,
                    CreatedAt = order.CreatedAt,
                    OrderItems = order.OrderItems,
                    Status = order.OrderStatus.Name
                };
                return history;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrderDto>> GetBilledOrdersAsync()
        {
            var orders = await orderRepository.GetOrdersByStatusAsync("Billed");
            return mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<List<OrderDto>> GetDeliveredOrdersByEmployeeAsync(ClaimsIdentity user)
        {
            var employeeId = Guid.Parse(user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var orders = await orderRepository.GetDeliveredOrdersByEmployeeAsync(employeeId);
            return mapper.Map<List<OrderDto>>(orders);
        }
    }
}

//