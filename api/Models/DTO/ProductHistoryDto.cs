using APIPractice.Models.Domain;

namespace APIPractice.Models.DTO
{
    public class ProductHistoryDto
    {
        public required Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
