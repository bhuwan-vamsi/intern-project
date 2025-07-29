using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class CustomerCategoryDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string ImageUrl { get; set; }
    }
}
