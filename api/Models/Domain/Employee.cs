using APIPractice.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIPractice.Models.Domain
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; } // This Id will be extracted from Identity User

        public required string Name { get; set; }

        public Guid? ManagerId { get; set; }

        public required bool IsActive { get; set; }


        // Navigation Property
        [ForeignKey(nameof(ManagerId))]
        public Manager? Manager { get; set; }
    }
}
