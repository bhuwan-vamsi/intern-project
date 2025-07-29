using APIPractice.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.Domain
{
    [Index(nameof(Phone), IsUnique = true)]
    public class Customer
    {
        [Key]
        public Guid Id { get; set; } // This Id will be extracted from Identity User

        public required string Name { get; set; }

        public required string Phone { get; set; }

        public string? Address { get; set; }

        public required bool IsActive { get; set; }


        // Reverse Navigation Properties
        public ICollection<Order?> Orders { get; set; } = new List<Order?>();
    }
}
