using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class RegisterEmployeeRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required]
        public required string Role { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
