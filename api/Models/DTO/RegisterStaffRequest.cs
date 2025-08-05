using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class RegisterStaffRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required]
        public required string Role { get; set; }
    }
}
