using APIPractice.Data;
using APIPractice.Models.Domain;
using APIPractice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIPractice.Repository
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly ApplicationDbContext db;

        public OrderStatusRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Task<OrderStatus> AddAsync(OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> GetIdOfStatus(string name)
        {
            OrderStatus? status = await db.OrderStatuses.FirstOrDefaultAsync(x => x.Name.ToLower() == name);
            return status.Id;
        }

        public async Task<OrderStatus> GetOrderStatusById(Guid id)
        {
            OrderStatus? status = await db.OrderStatuses.FirstOrDefaultAsync(x => x.Id == id);
            return status;
        }

        //public async Task<OrderStatus> AddAsync(OrderStatus orderStatus)
        //{
        //    await db.OrderStatuses.AddAsync(orderStatus);
        //    await db.SaveChangesAsync();
        //    return orderStatus;
        //}
    }
}
