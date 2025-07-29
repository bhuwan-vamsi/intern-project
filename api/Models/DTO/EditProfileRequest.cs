using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models.DTO
{
    public class EditProfileRequest
    {
        [Required]
        public required string Name { get; set; }

        public required string Phone { get; set; }

        [Required]
        public required string Address { get; set; } 
    }
}
