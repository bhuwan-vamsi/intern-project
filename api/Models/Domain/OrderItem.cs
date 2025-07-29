using APIPractice.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPractice.Models.Domain
{
    [Index(nameof(OrderId), nameof(ProductId), IsUnique = true)]
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }


        // Navigation Properties
        //[ForeignKey(nameof(OrderId))]
        //public Order Order { get; set; }

        [ForeignKey(nameof(ProductId))]
        public required Product Product { get; set; }
    }
}
