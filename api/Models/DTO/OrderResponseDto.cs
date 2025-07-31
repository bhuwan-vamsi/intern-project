using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class OrderResponseDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity cannot be 0")]
        public int Quantity { get; set; }
        [Required]
        [Range(1, 100000, ErrorMessage = "Unit Price cannot be 0")]
        public decimal Price { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
