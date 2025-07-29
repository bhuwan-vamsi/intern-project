using APIPractice.Models.Domain;

namespace APIPractice.Repository.IRepository
{
    public interface IOrderStatusRepository
    {
        public Task<OrderStatus> AddAsync(OrderStatus orderStatus);

        public Task<Guid> GetIdOfStatus(string name);

        public Task<OrderStatus> GetOrderStatusById(Guid id);
    }
}
