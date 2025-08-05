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
        private readonly IStockRepository stockRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public StatisticService(IProductRepository<Product> productRepository, IOrderItemRepository orderItemRepository, IStockRepository stockRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.orderItemRepository = orderItemRepository;
            this.stockRepository = stockRepository;
            this.orderRepository = orderRepository;
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

        public async Task<Dictionary<string, ProductAnalysisDto>> ProductPriceAnalysis(Guid id)
        {
            var productAnalysisList = new Dictionary<string, ProductAnalysisDto>();
            var sellingPrice = await orderItemRepository.SellingPrices(id);
            var costPrice = await stockRepository.CostPrice(id);
            var costQueue = new Queue<SellingPrice>(costPrice.OrderBy(u=> u.Month));
            var sellingPriceSorted = sellingPrice.OrderBy(u => u.Month).ToList();
            decimal totalProfit = 0;
            foreach(var sale in sellingPriceSorted)
            {
                int remaingSoldQuantity = sale.Quantity;
                decimal saleRevenue = sale.Price * sale.Quantity;
                decimal cost = 0;
                while(remaingSoldQuantity > 0 && costQueue.Count > 0)
                {
                    var costItem = costQueue.Peek();
                    var quantityUsed = Math.Min(remaingSoldQuantity, costItem.Quantity);
                    cost += costItem.Price * quantityUsed;
                    remaingSoldQuantity -= quantityUsed;
                    costItem.Quantity -= quantityUsed;
                    if (costItem.Quantity <= 0)
                    {
                        costQueue.Dequeue();
                    }
                }
                var profit = (saleRevenue - cost);
                totalProfit += profit;
                if(!productAnalysisList.ContainsKey(sale.Month.ToString("MMM")))
                {
                    productAnalysisList[sale.Month.ToString("MMM")] = new ProductAnalysisDto
                    {
                        TotalRevenue = saleRevenue,
                        Profit = profit,
                        CostPrice = cost,
                    };
                }
                else
                {
                    productAnalysisList[sale.Month.ToString("MMM")].TotalRevenue += saleRevenue;
                    productAnalysisList[sale.Month.ToString("MMM")].Profit += profit;
                    productAnalysisList[sale.Month.ToString("MMM")].CostPrice += cost;
                }
            }
            return productAnalysisList;
        }

        public async Task<RevenueAnalysisDto> RevenueAnalysis()
        {
            var orders = await orderRepository.GetAllOrderList();
            var deliveredOrders = orders.Where(u => u.DeliveredAt != null);
            var revenueAnalysis = new RevenueAnalysisDto
            {
                TotalSales = deliveredOrders.Sum(o => o.Amount),
                AvgOrderValue = deliveredOrders.Sum(o=> o.Amount) / deliveredOrders.Count(),
                ActiveCustomers = orders.Select(o => o.CustomerId).Distinct().Count(),
                TotalOrders = orders.Count(),
                PendingOrders = orders.Where(o => o.DeliveredAt == null).Count(),
                CompletedOrders = deliveredOrders.Count()
            };
            revenueAnalysis.YearlyRevenue = await orderRepository.GetTotalSalesByMonth();
            return revenueAnalysis;
        }
    }
}
