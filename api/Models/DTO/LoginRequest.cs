using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class LoginRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
