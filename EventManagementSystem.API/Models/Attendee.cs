
namespace EventManagementSystem.API.Models
{
    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public ICollection<EventAttendee> EventAttendees { get; set; } = new List<EventAttendee>();
    }
}