using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using System.Security.Claims;

namespace APIPractice.Services.IService
{
    public interface IOrderService
    {
        public Task<List<ItemResponseDto>> CheckOut(PurchaseOrderRequest orders, Guid userId);

        public Task<List<OrderHistoryDto>> ViewHistory(Guid Id);

        public Task<OrderHistoryDto?> ViewOrderById(Guid id, Guid userId);

        public Task<List<PurchaseOrderRequest>> GetBilledOrdersAsync();
        public Task<List<PurchaseOrderRequest>> GetDeliveredOrdersByEmployeeAsync(ClaimsIdentity user);
    }
}
