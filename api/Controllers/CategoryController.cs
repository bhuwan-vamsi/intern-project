using APIPractice.Models.DTO;
using APIPractice.Models.Responses;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace APIPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                List<CategoryDto> categories = await categoryService.GetCategories();
                return Ok(OkResponse<List<CategoryDto>>.Success(categories));
            }
            catch (Exception ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }
    }
}
