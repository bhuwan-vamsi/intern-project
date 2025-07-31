using APIPractice.Models.Domain;

namespace APIPractice.Repository.IRepository
{
    public interface IOrderItemRepository
    {
        public Task<OrderItem> AddAsync(OrderItem orderItem);
        public Task<List<OrderItem>> GetAllByIdAsync(Guid id);
        public Task<Product> GetMostSoldItem();
        public Task AddRangeAsync(List<OrderItem> orderItems);
    }
}
