using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.API.DTOs
{
    public class EventUpdateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required, MaxLength(200)]
        public string Location { get; set; } = null!;

        [MaxLength(200)]
        public string? Tags { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Capacity { get; set; }
    }
}