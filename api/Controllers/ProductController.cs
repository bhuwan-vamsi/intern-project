using APIPractice.CustomAcitonFilters;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using APIPractice.Services.IService;
using System.Security.Claims;
using System.Linq;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

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
        public async Task<IActionResult> GetAll([FromQuery] string? categoryName, [FromQuery] string? filterQuery)
        {
            try
            {
                var products = await productService.GetAllProductAsync(categoryName, filterQuery);

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
                        Category = p.Category,
                    }).ToList();

                    return Ok(productsCustomerDto);
                }

                if(role == "Manager")
                {
                    return Ok(products);
                }

                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Employee,Manager")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                return Ok(await productService.GetProductAsync(id));
            }
            catch (KeyNotFoundException ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto entity)
        {
            var product = await productService.CreateProductAsync(entity);
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
                return Ok("Updated Successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
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
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
