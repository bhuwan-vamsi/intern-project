using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class CreateProductDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public required string Units { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Threshold { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
    }
}
