using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class OrderItemDto
    {
        public string? ProductName { get; set; }
        public string? Units { get; set; }
        //public required string ImageUrl { get; set; }
        public string? Category { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid ProductId { get; set; }
    }
}
