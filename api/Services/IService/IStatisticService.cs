using APIPractice.Models.Domain;
using APIPractice.Models.DTO;

namespace APIPractice.Services.IService
{
    public interface IStatisticService
    {
        Task<InventorySummaryDto> InventorySummary();
        Task<CategoryDistributionDto> CategoryDistribution();
        Task<ProductDto> MostSoldProducts();
        Task<Dictionary<string,ProductAnalysisDto>> ProductPriceAnalysis(Guid id);
        Task<RevenueAnalysisDto> RevenueAnalysis();
    }
}
