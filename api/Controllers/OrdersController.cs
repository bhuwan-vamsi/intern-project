using APIPractice.CustomAcitonFilters;
using APIPractice.Exceptions;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Models.Responses;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace APIPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder([FromBody]PurchaseOrderRequest orders)
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                    ? id : throw new UnauthorizedAccessException("Invalid or missing user ID.");

                List<ItemResponseDto> statusList = await orderService.CheckOut(orders, userId);
                
                return Ok(OkResponse<List<ItemResponseDto>>.Success(statusList));
            }catch (BadRequestException ex)
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
                var userId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                    ? id : throw new UnauthorizedAccessException("Invalid or missing user ID.");

                List<OrderHistoryDto> history = await orderService.ViewHistory(userId);

                return (history.Count>0) ? Ok(OkResponse<List<OrderHistoryDto>>.Success(history)) 
                    : Ok(OkResponse<List<OrderHistoryDto>>.Empty());
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
                var userId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var tempid)
                    ? tempid : throw new UnauthorizedAccessException("Invalid or missing user ID.");

                OrderHistoryDto? history = await orderService.ViewOrderById(id,userId);

                return (history != null) ? Ok(OkResponse<OrderHistoryDto>.Success(history))
                    : Ok(OkResponse<OrderHistoryDto>.Empty());
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