using APIPractice.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPractice.Models.DTO
{
    public class OrderHistoryDto
    {
        public required Guid Id { get; set; }
        public required string Status { get; set; }
        public required string Name { get; set; }
        public required string Mobile { get; set; }
        public string? Address { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime? DeliveredAt { get; set; }
        public ICollection<OrderResponseDto> Items { get; set; } = new List<OrderResponseDto>();
        public int TotalItems { get; set; }
        public required BillingDto Billing { get; set; }

        

    }
}
