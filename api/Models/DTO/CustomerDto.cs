namespace APIPractice.Models.DTO
{
    public class CustomerDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }

        public required string PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
