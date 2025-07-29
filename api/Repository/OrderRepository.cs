using APIPractice.Data;
using APIPractice.Models.Domain;
using APIPractice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIPractice.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext db;

        public OrderRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Order> AddAsync(Order order)
        {
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderOfCustomer(Guid orderId)
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
        }

        public async Task<List<Order>> GetOrderHistoryOfCustomer(Guid customerId)
        {
            List<Order> orders = await db.Orders.Include("OrderItems").Include("OrderStatus").Include("OrderItems.Product").
                Include("OrderItems.Product.Category").Where(x=>x.CustomerId == customerId).ToListAsync();

            if(orders == null || orders.Count == 0)
            {
                throw new Exception("No History Found");
            }
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId, Guid customerId)
        {
            Order? order = await db.Orders.Include("OrderItems").Include("OrderStatus").Include("OrderItems.Product").
                Include("OrderItems.Product.Category").FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Id == orderId);

            if(order == null)
            {
                throw new Exception("No Order Found");
            }
            return order;
        }

        public async Task UpdateAsync(Guid id,Order order)
        {
            Order? existingOrder = await db.Orders.FirstOrDefaultAsync(o=> o.Id == id);
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
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            var statusEntity = await db.OrderStatuses.FirstOrDefaultAsync(s => s.Name == status);
            return await db.Orders
                .Where(o => o.OrderStatus == statusEntity)
                .Include(o => o.OrderItems)
                .ToListAsync();
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

    }
}
