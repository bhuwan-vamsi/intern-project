using APIPractice.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class ProductDto : ProductCustomerDto
    {
        public int Threshold { get; set; }
        public bool IsActive { get; set; }
    }
}
