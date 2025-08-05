using APIPractice.CustomAcitonFilters;
using APIPractice.Models.DTO;
using APIPractice.Models.Responses;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("view-profile")]
        [ValidateModel]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ViewProfile()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    return Unauthorized(BadResponse<string>.Execute("User identity not found."));
                }

                var customer = await customerService.ViewProfile(identity);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }

        [HttpPut("edit-profile")]
        [ValidateModel]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileRequest request)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    return Unauthorized(BadResponse<string>.Execute("User identity not found."));
                }

                await customerService.EditProfile(identity, request);
                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }
    }
}
