namespace APIPractice.Models.DTO
{
    public class ProductAnalysisDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal Profit { get; set; }
        public decimal CostPrice { get; set; }
        public decimal ProfitPercentage => CostPrice == 0 ? 0 : (Profit / CostPrice) * 100;
    }
}
