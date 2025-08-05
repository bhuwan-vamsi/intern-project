using APIPractice.Models.Domain;
using APIPractice.Models.DTO;

namespace APIPractice.Repository.IRepository
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> AddAsync(OrderItem orderItem);
        Task<List<OrderItem>> GetAllByIdAsync(Guid id);
        Task<Product> GetMostSoldItem();
        Task AddRangeAsync(List<OrderItem> orderItems);
        Task<List<SellingPrice>> SellingPrices(Guid id);
    }
}
