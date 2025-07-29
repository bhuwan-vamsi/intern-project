using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIPractice.Models.Domain
{
    [Index(nameof(Name), IsUnique = true)]
    public class OrderStatus
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }


        //// Reverse Navigation Properties
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
