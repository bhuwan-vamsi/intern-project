using APIPractice.Models.DTO;

namespace APIPractice.Repository.IRepository
{
    public interface IStockRepository
    {
        Task<List<SellingPrice>> CostPrice(Guid id);
    }
}
