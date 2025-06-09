
namespace EventManagementSystem.API.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public int Capacity { get; set; }
        public int RemainingCapacity { get; set; }
        public string? Tags { get; set; }

        public ICollection<EventAttendee> EventAttendees { get; set; } = new List<EventAttendee>();
    }
}