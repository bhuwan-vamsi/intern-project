using APIPractice.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class ProductCustomerDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required]
        public required string Units { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        public required Category Category { get; set; }
    }
}
