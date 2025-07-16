namespace APIPractice.DTOs
{
    public class CreateItemDto
    {
        public string Itemname { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
