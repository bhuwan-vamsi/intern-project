namespace APIPractice.Models.DTO
{
    public class OrderResponseDto : OrderItemDto
    {
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
