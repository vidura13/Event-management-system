using EventManagementSystem.API.DTOs;

namespace EventManagementSystem.API.Services
{
    public interface IEventService
    {
        Task<(IEnumerable<EventDto> Events, int TotalCount)> GetEventsAsync(
            int page, int pageSize, DateTime? date = null, string? location = null, string? tags = null, string? name = null, string? sortDate = "desc", string? datefilterType = null);

        Task<EventDto?> GetEventByIdAsync(int eventId);

        Task<EventDto> CreateEventAsync(EventCreateDto dto);

        Task<bool> UpdateEventAsync(int eventId, EventUpdateDto dto);

        Task<bool> DeleteEventAsync(int eventId);

        Task<bool> RegisterAttendeeAsync(int eventId, AttendeeCreateDto dto);

        Task<IEnumerable<AttendeeDto>> GetAttendeesForEventAsync(int eventId);

        Task<EventAnalyticsDto> GetEventAnalyticsAsync(int eventId);
    }
}