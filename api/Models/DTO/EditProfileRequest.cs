using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class EditProfileRequest
    {
        public string? Name { get; set; }

        public required string PhoneNumber { get; set; }

        public string? Address { get; set; } 
    }
}
