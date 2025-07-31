using APIPractice.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class PurchaseOrderRequest
    {
        [Required]
        [MinLength(1, ErrorMessage ="Items cannot be 0")]
        public required IEnumerable<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
