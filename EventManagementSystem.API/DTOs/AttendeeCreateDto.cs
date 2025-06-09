using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.API.DTOs
{
    public class AttendeeCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; } = null!;
    }
}