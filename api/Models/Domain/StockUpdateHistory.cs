using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPractice.Models.Domain
{
    public class StockUpdateHistory
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid ManagerId { get; set; }
        public decimal Price { get; set; }

        public int QuantityIn { get; set; }
        public int QuantityRemaining { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public Manager? Manager { get; set; }
    }
}
