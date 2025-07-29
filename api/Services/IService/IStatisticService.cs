using APIPractice.Models.DTO;

namespace APIPractice.Services.IService
{
    public interface IStatisticService
    {
        Task<InventorySummaryDto> InventorySummary();
        Task<CategoryDistributionDto> CategoryDistribution();
    }
}
