using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using System.Security.Claims;

namespace APIPractice.Services.IService
{
    public interface IOrderService
    {
        public Task CheckOut(OrderDto orders, ClaimsIdentity identity);

        public Task<List<OrderHistoryDto>> ViewHistory(ClaimsIdentity claimsIdentity);

        public Task<OrderHistoryDto> ViewOrderById(Guid id, ClaimsIdentity claimsIdentity);

        public Task<List<OrderDto>> GetBilledOrdersAsync();
        public Task<List<OrderDto>> GetDeliveredOrdersByEmployeeAsync(ClaimsIdentity user);
    }
}
