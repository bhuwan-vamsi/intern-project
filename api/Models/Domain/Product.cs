using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIPractice.Models.Domain
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Units { get; set; }

        public required int Quantity { get; set; }

        public required decimal Price { get; set; }

        public required int Threshold { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

        public bool IsActive { get; set; }


        // Navigation Property
        [ForeignKey(nameof(CategoryId))]
        public required Category Category { get; set; }


        // Reverse Navigation Properties
        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public ICollection<StockUpdateHistory> StockUpdateHistories { get; set; } = new List<StockUpdateHistory>();
    }
}
