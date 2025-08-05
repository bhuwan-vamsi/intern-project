using APIPractice.Data;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIPractice.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext db;

        public StockRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<List<SellingPrice>> CostPrice(Guid id)
        {
            var stock = db.StockUpdateHistories.Include("Product").AsQueryable();
            var costPrice = await stock
                            .Where(s => s.ProductId == id)
                            .Select(s => new
                            {
                                s.QuantityIn,
                                s.Price,
                                CreatedMonth = new DateTime(s.UpdatedAt.Year, s.UpdatedAt.Month, 1)
                            })
                            .Where(s => s.CreatedMonth >= DateTime.Now.AddMonths(-12)) // Filter for the last 12 months
                            .OrderBy(s => s.CreatedMonth)
                            .GroupBy(s => new { s.Price, s.CreatedMonth })
                            .Select(g => new SellingPrice
                            {
                                Quantity = g.Sum(x=> x.QuantityIn),
                                Price = g.Key.Price,
                                Month = g.Key.CreatedMonth
                            })
                            .ToListAsync();
            return costPrice;
        }
    }
}
