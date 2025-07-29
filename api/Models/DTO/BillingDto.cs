namespace APIPractice.Models.DTO
{
    public class BillingDto
    {
        public decimal ItemTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalBill {  get; set; }
        public decimal PlatformFee { get; set; }
    }
}
