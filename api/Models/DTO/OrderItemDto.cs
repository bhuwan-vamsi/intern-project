using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class OrderItemDto
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        [Range(1,int.MaxValue, ErrorMessage="Quantity cannot be 0")]
        public int Quantity { get; set; }
        [Required]
        [Range(1, 100000, ErrorMessage ="Unit Price cannot be 0")]
        public decimal UnitPrice { get; set; }
    }
}
