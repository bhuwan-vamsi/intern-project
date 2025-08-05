using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class RegisterCustomerRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }
    }
}
