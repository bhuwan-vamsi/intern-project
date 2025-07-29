using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIPractice.Models.Domain
{
    public class Manager
    {
        [Key]
        public Guid Id { get; set; } // This Id will be extracted from Identity User

        public required string Name { get; set; }


        // Reverse Navigation Properties
        [JsonIgnore]
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        [JsonIgnore]
        public ICollection<StockUpdateHistory> StocksHistory { get; set; } = new List<StockUpdateHistory>();
    }
}
