using APIPractice.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class ProductDto : ProductCustomerDto
    {
        public required int Quantity { get; set; }
        public int Threshold { get; set; }
        public required string ProductStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
