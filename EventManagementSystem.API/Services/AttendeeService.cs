using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.API.Services
{
    public class AttendeeService : IAttendeeService
    {
        private readonly EventManagementDbContext _context;

        public AttendeeService(EventManagementDbContext context)
        {
            _context = context;
        }

        public async Task<AttendeeDto?> GetAttendeeByIdAsync(int attendeeId)
        {
            var attendee = await _context.Attendees.FindAsync(attendeeId);
            if (attendee == null) return null;

            return new AttendeeDto
            {
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email
            };
        }




        public async Task<IEnumerable<EventDto>> GetRegisteredEventsAsync(int attendeeId)
        {
            return await _context.EventAttendees
                .Where(ea => ea.AttendeeId == attendeeId)
                .Include(ea => ea.Event)
                .Select(ea => new EventDto
                {
                    Id = ea.Event.Id,
                    Name = ea.Event.Name,
                    Description = ea.Event.Description,
                    Date = ea.Event.Date,
                    Location = ea.Event.Location,
                    CreatedBy = ea.Event.CreatedBy,
                    Capacity = ea.Event.Capacity,
                    RemainingCapacity = ea.Event.RemainingCapacity,
                    Tags = ea.Event.Tags
                }).ToListAsync();
        }
    }
}