using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using static APIPractice.Models.DTO.CategoryDistributionDto;

namespace APIPractice.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IProductRepository<Product> productRepository;

        public StatisticService(IProductRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<CategoryDistributionDto> CategoryDistribution()
        {
            var categories = await productRepository.GetAllCategoriesAsync();
            var products = await productRepository.GetAllAsync();
            var categoryDistribution = new CategoryDistributionDto();
            categoryDistribution.TotalCategories = categories.Count();
            categoryDistribution.TotalItemsInCategories = products.Count();
            int quantity = 0;
            foreach (Category category in categories)
            {
                categoryDistribution.Categories.Add(new CategoriesList { Name = category.Name, Items = products.Where(u => u.Category == category).Count(), IsNew = false});
                quantity += products.Where(p=> p.CategoryId == category.Id).Sum(p => p.Quantity);
            }
            categoryDistribution.TotalQuantityInCategories = quantity;
            categoryDistribution.LastUpdated = DateTime.UtcNow;
            return categoryDistribution;
        }

        public async Task<InventorySummaryDto> InventorySummary()
        {
            var invSummary = new InventorySummaryDto();
            var products = await productRepository.GetAllAsync();
            invSummary.TotalItems = products.Count();
            invSummary.Instock = products.Count(p => p.Quantity > p.Threshold);
            invSummary.LowStock = products.Count(p => p.Quantity <= p.Threshold && p.Quantity>0);
            invSummary.OutOfStock = products.Count(p => p.Quantity == 0);
            invSummary.LastUpdated = DateTime.UtcNow;
            return invSummary;
        }
    }
}
