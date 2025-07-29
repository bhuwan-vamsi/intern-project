using APIPractice.CustomAcitonFilters;
using APIPractice.Models.DTO;
using APIPractice.Services;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [ValidateModel]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllEmployee()
        {
            return Ok(await adminService.GetAllEmployee());
        }
        [HttpGet]
        [Route("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetEmployeeById([FromRoute] Guid id)
        {
            try
            {
                var employee = await adminService.GetEmployee(id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("RegisterEmployee")]
        [ValidateModel]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Regsiter([FromBody] RegisterEmployeeRequest registerEmployeeRequest)
        {
            try
            {
                await adminService.RegisterEmployee(registerEmployeeRequest);
                return Ok("User Registered");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("AssignManager")]
        [ValidateModel]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AssignManager([FromBody] UpdateEmployeeRequest updateEmployee)
        {
            try
            {
                await adminService.AssignManager(updateEmployee);
                return Ok("Manager Assigned");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
