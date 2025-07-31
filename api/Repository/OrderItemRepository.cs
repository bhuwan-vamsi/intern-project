using APIPractice.Data;
using APIPractice.Models.Domain;
using APIPractice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace APIPractice.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext db;

        public OrderItemRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<OrderItem> AddAsync(OrderItem orderItem)
        {
            await db.OrderItems.AddAsync(orderItem);
            await db.SaveChangesAsync();
            return orderItem;
        }

        public async Task AddRangeAsync(List<OrderItem> orderItems)
        {
            await db.OrderItems.AddRangeAsync(orderItems);
            await db.SaveChangesAsync();

        }

        public async Task<List<OrderItem>> GetAllByIdAsync(Guid id)
        {
            List<OrderItem> orderItems = await db.OrderItems.Where(u=> u.Id == id).ToListAsync();
            if(orderItems == null)
            {
                throw new Exception("Order Items not found");
            }
            return orderItems;
        }

        public async Task<Product> GetMostSoldItem()
        {
            var orderItems = db.OrderItems.Include("Product").AsQueryable();
            if (orderItems == null)
            {
                throw new Exception("Order Items not found");
            }
            var mostSoldProduct = await orderItems
                                    .GroupBy(oi => oi.Product)
                                    .Select(group => new {Product =group.Key, TotalQuantity =group.Sum(g=>g.Quantity)})
                                    .OrderByDescending(x=> x.TotalQuantity)
                                    .Select(x=> x.Product)
                                    .FirstOrDefaultAsync();
            if(mostSoldProduct == null)
            {
                throw new KeyNotFoundException("No Products Found");
            }
            return mostSoldProduct;
        }
    }
}
