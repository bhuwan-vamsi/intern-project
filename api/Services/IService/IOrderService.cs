using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using System.Security.Claims;

namespace APIPractice.Services.IService
{
    public interface IOrderService
    {
        public Task CheckOut(PurchaseOrderRequest orders, ClaimsIdentity identity);

        public Task<List<OrderHistoryDto>> ViewHistory(ClaimsIdentity claimsIdentity);

        public Task<OrderHistoryDto> ViewOrderById(Guid id, ClaimsIdentity claimsIdentity);

        public Task<List<PurchaseOrderRequest>> GetBilledOrdersAsync();
        public Task<List<PurchaseOrderRequest>> GetDeliveredOrdersByEmployeeAsync(ClaimsIdentity user);
    }
}
