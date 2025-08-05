using APIPractice.Data;
using APIPractice.Infrastructure;
using APIPractice.Models.Domain;
using APIPractice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APIPractice.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext db;
        private readonly TransactionManager transactionManager;

        public OrderRepository(ApplicationDbContext db, TransactionManager transactionManager)
        {
            this.db = db;
            this.transactionManager = transactionManager;
        }

        public async Task<Order> AddAsync(Order order)
        {
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderOfCustomer(Guid orderId)
        {
            await transactionManager.ExecuteInTransactionAsync(async () =>
            {
                Order? order = await db.Orders.Include("OrderItems").Include("OrderStatus").FirstOrDefaultAsync(x => x.Id == orderId);
                if (order == null)
                {
                    throw new Exception("Order not found");
                }
                db.Orders.RemoveRange(order);


                List<OrderItem> items = await db.OrderItems.Where(x => x.OrderId == orderId).ToListAsync();
                db.OrderItems.RemoveRange(items);

                await db.SaveChangesAsync();
            });
        }

        public async Task<List<Order>> GetOrderHistoryOfCustomer(Guid customerId)
        {
            List<Order> orders = await db.Orders
                                .Include(o => o.OrderItems)
                                .ThenInclude(oi => oi.Product)
                                .ThenInclude(p => p.Category)
                                .Include(o => o.OrderStatus)
                                .Include(o => o.Customer)
                                .Where(o => o.CustomerId == customerId)
                                .OrderBy(o => o.CreatedAt)
                                .ToListAsync();

            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId, Guid customerId)
        {
            Order? order = await db.Orders.Include("OrderItems").Include("OrderStatus").Include("OrderItems.Product").
                Include("OrderItems.Product.Category").Include("Customer")
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Id == orderId);

            return order;
        }

        public async Task UpdateAsync(Guid id, Order order)
        {
            await transactionManager.ExecuteInTransactionAsync(async () =>
            {
                Order? existingOrder = await db.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (existingOrder == null)
                {
                    throw new Exception("The order could not be updated");
                }
                existingOrder.CustomerId = order.CustomerId;
                existingOrder.Customer = order.Customer;
                existingOrder.Amount = order.Amount;
                existingOrder.OrderStatus = order.OrderStatus;
                existingOrder.OrderStatusId = order.OrderStatusId;
                existingOrder.CreatedAt = order.CreatedAt;
                existingOrder.OrderItems = order.OrderItems;
                db.Orders.Update(existingOrder);

                await db.SaveChangesAsync();
            });
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            try
            {
                Console.WriteLine($"[Repository] Looking for status: {status}");

                var statusEntity = await db.OrderStatuses.FirstOrDefaultAsync(s => s.Name == status);

                if (statusEntity == null)
                {
                    Console.WriteLine($"[Repository] Status '{status}' not found in OrderStatuses.");
                    return new List<Order>();
                }

                return await db.Orders
                    .Where(o => o.OrderStatusId == statusEntity.Id)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Category)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Repository] Exception: {ex.Message}");
                return new List<Order>();
            }
        }

        public async Task<List<Order>> GetDeliveredOrdersByEmployeeAsync(Guid employeeId)
        {
            var orderIds = await db.TasksHistory
                .Where(t => t.EmployeeId == employeeId && t.CompletedAt != null)
                .Select(t => t.OrderId)
                .ToListAsync();

            return await db.Orders
                .Where(o => orderIds.Contains(o.Id))
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrderList()
        {
            var orders = await db.Orders.ToListAsync();
            return orders;
        }
        public async Task<Dictionary<string, decimal>> GetTotalSalesByMonth()
        {
            var salesData = await db.Orders
                .Where(o => o.DeliveredAt != null)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"),
                    TotalSales = g.Sum(o => o.Amount)
                })
                .ToListAsync();
            return salesData.ToDictionary(x => x.Month, x => x.TotalSales);
        }
    }
}
