using APIPractice.CustomAcitonFilters;
using APIPractice.Exceptions;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpPost("create-order")]
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
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }

        }

        [HttpGet("order-history")]
        [ValidateModel]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ViewOrderHistory()
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                    ? id : throw new UnauthorizedAccessException("Invalid or missing user ID.");

                List<OrderHistoryDto> history = await orderService.ViewHistory(userId);

                return history.Count > 0
                    ? Ok(OkResponse<List<OrderHistoryDto>>.Success(history))
                    : NoContent();
            }catch(Exception ex)
            {
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }

        [HttpGet("order-history/{id:Guid}")]
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
                return BadRequest(BadResponse<string>.Execute(ex.Message));
            }
        }

        [HttpGet("billed-orders")]
        [ValidateModel]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetBilledOrders()
        {
            try
            {
                Console.WriteLine("[Controller] Request received to fetch billed orders");

                var orders = await orderService.GetBilledOrdersAsync();

                if (orders == null || orders.Count == 0)
                {
                    Console.WriteLine("[Controller] No billed orders found.");
                    return Ok("No billed orders found.");
                }

                Console.WriteLine($"[Controller] Returning {orders.Count} billed orders");
                return Ok(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Controller] Exception: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching billed orders.");
            }
        }

        [HttpGet("delivered-orders")]
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
