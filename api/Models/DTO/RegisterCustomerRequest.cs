using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class RegisterCustomerRequest : RegisterEmployeeRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public required string Phone { get; set; }
    }
}
