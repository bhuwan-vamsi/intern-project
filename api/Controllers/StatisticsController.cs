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
            try
            {
                var inventorySummary = await statisticService.InventorySummary();
                return Ok(inventorySummary);
            }
            catch (Exception) {
                return BadRequest("An error occurred while fetching the inventory summary.");
            }
        }
        [HttpGet]
        [Route("CategoryDistribution")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCategoryDistribuiton()
        {
            try
            {
                var categoryDistribution = await statisticService.CategoryDistribution();
                return Ok(categoryDistribution);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while fetching the category distribution.");
            }
        }
        [HttpGet]
        [Route("FastMovingProduct")]
        public async Task<IActionResult> GetFastMovingProducts()
        {
            try
            {
                var fastMovingProduct = await statisticService.MostSoldProducts();
                return Ok(fastMovingProduct);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No fast moving product found.");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while fetching the fast moving product.");
            }
        }
    }
}
