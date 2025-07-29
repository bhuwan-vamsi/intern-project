using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class CategoryDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
