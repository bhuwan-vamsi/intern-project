using APIPractice.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPractice.Models.DTO
{
    public class OrderHistoryDto
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public decimal Amount { get; set; }

        public required string Status { get; set; }

    }
}
