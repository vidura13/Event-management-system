namespace EventManagementSystem.API.DTOs
{
    public class AttendeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}