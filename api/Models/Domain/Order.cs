using APIPractice.Models.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPractice.Models.Domain
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public decimal Amount { get; set; }

        public Guid OrderStatusId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }



        // Navigation Properties
        [ForeignKey(nameof(CustomerId))]
        public required Customer Customer { get; set; }

        [ForeignKey(nameof(OrderStatusId))]
        public required OrderStatus OrderStatus { get; set; }


        // Reverse Navigation Properties
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
    }
}
