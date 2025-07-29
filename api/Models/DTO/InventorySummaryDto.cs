namespace APIPractice.Models.DTO
{
    public class InventorySummaryDto
    {
        public int TotalItems { get; set; }
        public decimal ChangeInPercentage { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Instock { get; set; }
        public int OutOfStock { get; set; }
        public int LowStock { get; set; }
    }
}
