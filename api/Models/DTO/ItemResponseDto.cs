using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace APIPractice.Models.DTO
{
    public class ItemResponseDto
    {
        public Guid productId { get; set; }
        [Required]
        public required string status { get; set; }
        [AllowNull]
        public string? error { get; set; }

    }
}
