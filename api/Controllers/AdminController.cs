using APIPractice.CustomAcitonFilters;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Models.Responses;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet("employees")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await adminService.GetAllEmployee();
                return Ok(OkResponse<IEnumerable<Employee>>.Success(employees));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BadResponse<string>.Execute($"An error occurred while fetching employees: {ex.Message}"));
            }
        }

        [HttpGet("employees/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] Guid id)
        {
            try
            {
                var employee = await adminService.GetEmployee(id);
                return Ok(OkResponse<Employee>.Success(employee));
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(BadResponse<string>.Execute(knfEx.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BadResponse<string>.Execute($"An error occurred while fetching employee: {ex.Message}"));
            }
        }

        [HttpPut("employees/assign-manager")]
        [ValidateModel]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignManager([FromBody] UpdateEmployeeRequest updateEmployee)
        {
            try
            {
                await adminService.AssignManager(updateEmployee);
                return Ok(OkResponse<string>.Success("Manager assigned successfully."));
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(BadResponse<string>.Execute(argEx.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BadResponse<string>.Execute($"An error occurred while assigning manager: {ex.Message}"));
            }
        }
    }
}
