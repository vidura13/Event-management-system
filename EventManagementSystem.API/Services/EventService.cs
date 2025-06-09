using EventManagementSystem.API.DTOs;
using EventManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.API.Services
{
    public class EventService : IEventService
    {
        private readonly EventManagementDbContext _context;

        public EventService(EventManagementDbContext context)
        {
            _context = context;
        }

        // get all events
        public async Task<(IEnumerable<EventDto>, int)> GetEventsAsync(
    int page, int pageSize, DateTime? date = null, string? location = null, string? tags = null, string? name = null, string? sortDate = "desc", string? datefilterType = null)
        {
            var query = _context.Events
                .Include(e => e.EventAttendees).ThenInclude(ea => ea.Attendee)
                .AsQueryable();

            if (date.HasValue)
                query = query.Where(e => e.Date.Date == date.Value.Date);

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(e => e.Location.Contains(location));

            if (!string.IsNullOrWhiteSpace(tags))
                query = query.Where(e => e.Tags != null && e.Tags.Contains(tags));

            if (!string.IsNullOrEmpty(name))
                query = query.Where(e => e.Name.Contains(name));

            //filtering
            if (datefilterType == "upcoming")
            {
                query = query.Where(e => e.Date >= DateTime.Today)
                             .OrderBy(e => e.Date);
            }
            else if (datefilterType == "finished")
            {
                query = query.Where(e => e.Date < DateTime.Today)
                             .OrderByDescending(e => e.Date);
            }
            else
            {
                if (sortDate == "asc")
                {
                    query = query.OrderBy(e => e.Date);
                }
                else
                {
                    query = query.OrderByDescending(e => e.Date);
                }
            }

            var total = await query.CountAsync();
            
            //pagunation
            var events = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Date = e.Date,
                    Location = e.Location,
                    CreatedBy = e.CreatedBy,
                    Capacity = e.Capacity,
                    RemainingCapacity = e.RemainingCapacity,
                    Tags = e.Tags,
                    Attendees = e.EventAttendees.Select(ea => new AttendeeDto
                    {
                        Id = ea.Attendee.Id,
                        Name = ea.Attendee.Name,
                        Email = ea.Attendee.Email
                    }).ToList()
                })
                .ToListAsync();

            return (events, total);
        }


        // get event
        public async Task<EventDto?> GetEventByIdAsync(int eventId)
        {
            var evt = await _context.Events
                .Include(e => e.EventAttendees).ThenInclude(ea => ea.Attendee)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (evt == null) return null;

            return new EventDto
            {
                Id = evt.Id,
                Name = evt.Name,
                Description = evt.Description,
                Date = evt.Date,
                Location = evt.Location,
                CreatedBy = evt.CreatedBy,
                Capacity = evt.Capacity,
                RemainingCapacity = evt.RemainingCapacity,
                Tags = evt.Tags,
                Attendees = evt.EventAttendees.Select(ea => new AttendeeDto
                {
                    Id = ea.Attendee.Id,
                    Name = ea.Attendee.Name,
                    Email = ea.Attendee.Email
                }).ToList()
            };
        }
  

        // create event
        public async Task<EventDto> CreateEventAsync(EventCreateDto dto)
        {
            var ev = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                Date = dto.Date,
                Location = dto.Location,
                CreatedBy = dto.CreatedBy,
                Capacity = dto.Capacity,
                RemainingCapacity = dto.Capacity,
                Tags = dto.Tags
            };
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Description = ev.Description,
                Date = ev.Date,
                Location = ev.Location,
                CreatedBy = ev.CreatedBy,
                Capacity = ev.Capacity,
                RemainingCapacity = ev.RemainingCapacity,
                Tags = ev.Tags
            };
        }


        // event update
        public async Task<bool> UpdateEventAsync(int eventId, EventUpdateDto dto)
        {
            var ev = await _context.Events
                .Include(e => e.EventAttendees)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null || ev.Date < DateTime.UtcNow)
                return false;

            ev.Name = dto.Name;
            ev.Description = dto.Description;
            ev.Date = dto.Date;
            ev.Location = dto.Location;
            ev.Tags = dto.Tags;

            int attendeeCount = ev.EventAttendees.Count;

            ev.Capacity = dto.Capacity;
            ev.RemainingCapacity = Math.Max(0, ev.Capacity - attendeeCount);

            await _context.SaveChangesAsync();
            return true;
        }


        // delete event
        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var ev = await _context.Events
                .Include(e => e.EventAttendees)
                .FirstOrDefaultAsync(e => e.Id == eventId);
            if (ev == null) return false;
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return true;
        }

        //add attendee
        public async Task<bool> RegisterAttendeeAsync(int eventId, AttendeeCreateDto dto)
        {
            var ev = await _context.Events
                .Include(e => e.EventAttendees)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            // check if event is finished
            if (ev == null || ev.RemainingCapacity <= 0 || ev.Date < DateTime.UtcNow)
                return false;

            // check if event is full
            if (ev == null || ev.RemainingCapacity <= 0)
                return false;


            // Check if person already registered
            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(a => a.Email == dto.Email);

            if (attendee == null)
            {
                attendee = new Attendee { Name = dto.Name, Email = dto.Email };
                _context.Attendees.Add(attendee);
                await _context.SaveChangesAsync();
            }


            if (await _context.EventAttendees.AnyAsync(ea => ea.EventId == eventId && ea.AttendeeId == attendee.Id))
                return false;

            _context.EventAttendees.Add(new EventAttendee
            {
                EventId = eventId,
                AttendeeId = attendee.Id
            });
            ev.RemainingCapacity--;
            await _context.SaveChangesAsync();
            return true;
        }


        //get event attendees
        public async Task<IEnumerable<AttendeeDto>> GetAttendeesForEventAsync(int eventId)
        {
            return await _context.EventAttendees
                .Where(ea => ea.EventId == eventId)
                .Include(ea => ea.Attendee)
                .Select(ea => new AttendeeDto
                {
                    Id = ea.Attendee.Id,
                    Name = ea.Attendee.Name,
                    Email = ea.Attendee.Email
                }).ToListAsync();
        }


        // event analysis
        public async Task<EventAnalyticsDto> GetEventAnalyticsAsync(int eventId)
        {
            var ev = await _context.Events
                .Include(e => e.EventAttendees)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null) return new EventAnalyticsDto();

            int total = ev.EventAttendees.Count;
            double utilization = ev.Capacity == 0 ? 0 : (double)total / ev.Capacity;

            return new EventAnalyticsDto
            {
                EventId = ev.Id,
                TotalAttendees = total,
                CapacityUtilization = utilization
            };
        }
    }
}