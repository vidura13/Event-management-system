namespace EventManagementSystem.API.DTOs
{
    public class EventAnalyticsDto
    {
        public int EventId { get; set; }
        public int TotalAttendees { get; set; }
        public double CapacityUtilization { get; set; }
    }
}