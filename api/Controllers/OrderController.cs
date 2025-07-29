using APIPractice.CustomAcitonFilters;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpPost]
        [Route("CheckOut")]
        [ValidateModel]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CheckOut([FromBody]PurchaseOrderRequest orders)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                await orderService.CheckOut(orders, claimsIdentity);
                return Ok("Your Order was successfull");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("ViewHistory")]
        [ValidateModel]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ViewHistory()
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                List<OrderHistoryDto> history = await orderService.ViewHistory(claimsIdentity);
                return Ok(history);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("VewOrder/{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> ViewOrderById([FromRoute] Guid id)
        {
            try
            {
                var identityUser = (ClaimsIdentity)User.Identity;
                OrderHistoryDto order = await orderService.ViewOrderById(id,identityUser);
                return Ok(order);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BilledOrders")]
        [ValidateModel]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetBilledOrders()
        {
            var orders = await orderService.GetBilledOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("DeliveredByMe")]
        [ValidateModel]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetDeliveredOrdersByMe()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var orders = await orderService.GetDeliveredOrdersByEmployeeAsync(identity);
            return Ok(orders);
        }
    }
}
///<summar>
/// OrderDto
/// OrderItemsDTO
/// OrderDto -> List<OrderItemsDto>
/// 
/// var claimsIdentity = (ClaimsIdentity)User.Identity;
/// 
/// { {id=1,productname="babycare"} , {...} , {...} }
/// </summar>