using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using AutoMapper;
using static APIPractice.Models.DTO.CategoryDistributionDto;

namespace APIPractice.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IProductRepository<Product> productRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IMapper mapper;

        public StatisticService(IProductRepository<Product> productRepository, IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.orderItemRepository = orderItemRepository;
            this.mapper = mapper;
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
                categoryDistribution.Categories.Add(new CategoriesList { Name = category.Name, Items = category.Products.Count(), IsNew = false});
                quantity += category.Products.Sum(q=>q.Quantity);
            }
            categoryDistribution.TotalQuantityInCategories = quantity;
            categoryDistribution.LastUpdated = DateTime.Now.ToString("dd-MM-yy hh:mm tt");
            return categoryDistribution;
        }

        public async Task<ProductDto> MostSoldProducts()
        {
            var mostSoldProduct = await orderItemRepository.GetMostSoldItem();
            if (mostSoldProduct == null)
            {
                throw new KeyNotFoundException("No Product Foud");
            }
            return mapper.Map<ProductDto>(mostSoldProduct);
        }

        public async Task<InventorySummaryDto> InventorySummary()
        {
            var invSummary = new InventorySummaryDto();
            var products = await productRepository.GetAllAsync();
            invSummary.TotalItems = products.Count();
            invSummary.Instock = products.Count(p => p.Quantity > p.Threshold);
            invSummary.LowStock = products.Count(p => p.Quantity <= p.Threshold && p.Quantity>0);
            invSummary.OutOfStock = products.Count(p => p.Quantity == 0);
            invSummary.LastUpdated = DateTime.Now.ToString("dd-MM-yy hh:mm tt");
            return invSummary;
        }

    }
}
