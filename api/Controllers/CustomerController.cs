using APIPractice.Models.Domain;
using APIPractice.CustomAcitonFilters;
using APIPractice.Models.DTO;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using APIPractice.Models.Responses;

namespace APIPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        [HttpGet]
        [Route("ViewProfile")]
        [ValidateModel]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> ViewProfile()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    return Unauthorized("User identity not found.");
                }
                CustomerDto? customer = await customerService.ViewProfile(identity);
                return Ok(OkResponse<CustomerDto>.Success(customer));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("EditProfile")]
        [ValidateModel]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileRequest request)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if(identity == null)
                {
                    return Unauthorized("User identity not found.");
                }
                await customerService.EditProfile(identity, request);
                return NoContent();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories =  new List<CustomerCategoryDto>
                {
                    new CustomerCategoryDto
                    {
                        Id = Guid.Parse("75BD72C3-9150-4EC7-B2A9-A47E38744CAC"),
                        Name = "Baby Care",
                        ImageUrl = ""
                    },
                    new CustomerCategoryDto
                    {
                        Id = Guid.Parse("0100A0A6-8237-47E7-B158-7A40B4808E85"),
                        Name = "Bakery, Cakes & Dairy",
                        ImageUrl = ""
                    },
                    new CustomerCategoryDto
                    {
                        Id = Guid.Parse("D20B902C-05DF-4F4E-8647-639DE3319D5B"),
                        Name = "Beverages",
                        ImageUrl = ""
                    },
                    new CustomerCategoryDto
                    {
                        Id = Guid.Parse("5394E988-0A56-41C5-BD6A-3A0B9F2F3177"),
                        Name = "Eggs, Meat & Fish",
                        ImageUrl = ""
                    },
                    new CustomerCategoryDto
                    {
                        Id = Guid.Parse("B1F5A121-D4AD-48F2-8736-30BB6080B2CC"),
                        Name = "Fruits & Vegetables",
                        ImageUrl = ""
                    },
                    new CustomerCategoryDto
                    {
                        Id = Guid.Parse("5633EF72-1953-49B0-B587-95F8A2FC1684"),
                        Name = "Snacks & Branded Foods",
                        ImageUrl = ""
                    }
                };
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
