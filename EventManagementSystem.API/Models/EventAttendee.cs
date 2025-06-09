
namespace EventManagementSystem.API.Models
{
    public class EventAttendee
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int AttendeeId { get; set; }
        public Attendee Attendee { get; set; } = null!;
    }
}