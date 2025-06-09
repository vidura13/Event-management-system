using System;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.API.Services;
using EventManagementSystem.API.DTOs;

namespace EventManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }


        // get all events - GET
        [HttpGet]
        public async Task<IActionResult> GetEvents(
            int page = 1,
            int pageSize = 10,
            DateTime? date = null,
            string? location = null,
            string? tags = null,
            string? name = null,
            string? sortDate = "desc",
            string? datefilterType = null)
        {
            var (events, totalCount) = await _eventService.GetEventsAsync(page, pageSize, date, location, tags, name, sortDate, datefilterType);
            return Ok(new
            {
                data = events,
                totalCount
            });
        }


        // get an event by event id - GET 
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventById(int eventId)
        {
            var evt = await _eventService.GetEventByIdAsync(eventId);
            if (evt == null)
                return NotFound();
            return Ok(evt);
        }


        // add new event - POST 
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEvent = await _eventService.CreateEventAsync(dto);
            return CreatedAtAction(nameof(GetEventById), new { eventId = createdEvent.Id }, createdEvent);
        }


        // update an event - PUT 
        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEvent(int eventId, [FromBody] EventUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _eventService.UpdateEventAsync(eventId, dto);
            if (!updated)
                return NotFound();
            return NoContent();
        }


        // delete an event - DELETE 
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var deleted = await _eventService.DeleteEventAsync(eventId);
            if (!deleted)
                return NotFound();
            return NoContent();
        }


        // register a new attendee to event - POST 
        [HttpPost("{eventId}/register")]
        public async Task<IActionResult> RegisterAttendee(int eventId, [FromBody] AttendeeCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registered = await _eventService.RegisterAttendeeAsync(eventId, dto);

            if (!registered)
                return BadRequest(new { message = "Registration failed!. Event may be full, Attendee already registered, or The event is finished." });

            return Ok(new { message = "Registration successful" });
        }


        // get all attendees for an event - GET 
        [HttpGet("{eventId}/attendees")]
        public async Task<IActionResult> GetAttendeesForEvent(int eventId)
        {
            var attendees = await _eventService.GetAttendeesForEventAsync(eventId);
            return Ok(attendees);
        }


        // event analytics / utilization -  GET 
        [HttpGet("{eventId}/analytics")]
        public async Task<IActionResult> GetEventAnalytics(int eventId)
        {
            var analytics = await _eventService.GetEventAnalyticsAsync(eventId);
            return Ok(analytics);
        }
    }
}