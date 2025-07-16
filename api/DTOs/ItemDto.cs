namespace APIPractice.DTOs
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Itemname { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
