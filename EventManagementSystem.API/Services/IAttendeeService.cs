using EventManagementSystem.API.DTOs;

namespace EventManagementSystem.API.Services
{
    public interface IAttendeeService
    {
        Task<AttendeeDto?> GetAttendeeByIdAsync(int attendeeId);
        Task<IEnumerable<EventDto>> GetRegisteredEventsAsync(int attendeeId);
    }
}