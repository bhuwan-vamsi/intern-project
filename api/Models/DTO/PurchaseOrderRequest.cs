using APIPractice.Models.Domain;

namespace APIPractice.Models.DTO
{
    public class PurchaseOrderRequest
    {
        public IEnumerable<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
