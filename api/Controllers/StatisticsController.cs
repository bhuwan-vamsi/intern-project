using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            this.statisticService = statisticService;
        }
        [HttpGet]
        [Route("InventorySummary")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetInventorySummary()
        {
            var inventorySummary = await statisticService.InventorySummary();
            return Ok(inventorySummary);
        }
        [HttpGet]
        [Route("CategoryDistribution")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCategoryDistribuiton()
        {
            var categoryDistribution = await statisticService.CategoryDistribution();
            return Ok(categoryDistribution);
        }
    }
}
