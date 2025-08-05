using APIPractice.CustomAcitonFilters;
using APIPractice.Models.DTO;
using APIPractice.Models.Responses;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIPractice.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register-customer")]
        [ValidateModel]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequest registerCustomerRequest)
        {
            try
            {
                await authService.RegisterCustomer(registerCustomerRequest);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }

        [HttpPost("register-staff")]
        [ValidateModel]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterStaff([FromBody] RegisterStaffRequest registerStaffRequest)
        {
            try
            {
                await authService.RegisterStaff(registerStaffRequest);
                return StatusCode(StatusCodes.Status201Created, OkResponse<string>.Success("Staff registered successfully."));
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(BadResponse<string>.Execute(argEx.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BadResponse<string>.Execute($"An error occurred while registering staff: {ex.Message}"));
            }
        }

        [HttpPost("login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var response = await authService.LoginUser(loginRequest);
                return Ok(OkResponse<LoginResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }
    }
}
