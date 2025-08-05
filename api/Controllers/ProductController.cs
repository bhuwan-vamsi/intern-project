using APIPractice.CustomAcitonFilters;
using APIPractice.Models.DTO;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using APIPractice.Models.Responses;

namespace APIPractice.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [ValidateModel]
        [Authorize(Roles = "Customer,Manager")]
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] string? filterQuery, [FromQuery]string? sortBy,[FromQuery] bool IsAscending,[FromQuery] int PageNumber=1, [FromQuery] int PageSize = int.MaxValue)
        {
            try
            {
                var products = await productService.GetAllProductAsync(category, filterQuery,sortBy, IsAscending, PageNumber, PageSize);

                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (role == "Customer")
                {
                    var productsCustomerDto = products.Select(p => new ProductCustomerDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Units = p.Units,
                        ImageUrl = p.ImageUrl,
                        Category = p.Category
                    }).ToList();

                    return Ok(productsCustomerDto);
                }

                if (role == "Manager")
                {
                    return Ok(products);
                }

                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
            
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Customer,Manager")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value??throw new UnauthorizedAccessException("Invalid User");
                var product = await productService.GetProductAsync(id, role);
                if (role == "Customer")
                {
                    var productCustomerDto = new ProductCustomerDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Units = product.Units,
                        ImageUrl = product.ImageUrl,
                        Category = product.Category
                    };
                    return Ok(OkResponse<ProductCustomerDto>.Success(productCustomerDto));
                }
                else if(role== "Manager")
                {
                    return Ok(OkResponse<ProductDto>.Success(product));
                }
                return Forbid();
            }
            catch (KeyNotFoundException ex) 
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto entity)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier)??throw new UnauthorizedAccessException("Invalid User");
            var product = await productService.CreateProductAsync(entity, Guid.Parse(managerId));
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, entity);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductDto entity)
        {

            try
            {
                var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(managerId == null)
                {
                    return BadRequest("Invalid Token");
                }
                await productService.UpdateProductAsync(id, entity, Guid.Parse(managerId));
                return Ok(OkResponse<string>.Success("Updated Successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            try
            {
                await productService.DeleteProductAsync(id);

                return Ok(OkResponse<string>.Success("Toggled Successfully!"));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }
    }
}
