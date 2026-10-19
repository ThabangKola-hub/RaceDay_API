//Events controller
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay_API.Data;
using RaceDay_API.DTOs;
using RaceDay_API.Models;
using System.Security.Claims;

namespace RaceDay_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/events (Anyone can view events)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events.Select(e => new EventResponseDto
            {
                EventId = e.EventId,
                Name = e.Name,
                Description = e.Description,
                EventDate = e.EventDate,
                Location = e.Location,
                DistanceKm = e.DistanceKm,
                EventType = e.EventType.ToString(),
                Status = e.Status.ToString(),
                OrganiserId = e.OrganiserId
            }));
        }

        // POST: api/events (Only Organisers)
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<ActionResult<EventResponseDto>> CreateEvent(EventCreateDto dto)
        {
            // Get the logged-in organiser's ID from the JWT token
            var organiserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                EventDate = dto.EventDate,
                Location = dto.Location,
                DistanceKm = dto.DistanceKm,
                EventType = dto.EventType,
                OrganiserId = organiserId,
                Status = EventStatus.Scheduled
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvents), new { id = newEvent.EventId }, newEvent);
        }

        // PUT: api/events/5 (Only Organisers)
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, EventCreateDto dto)
        {
            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null) return NotFound("Event not found.");

            var organiserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (existingEvent.OrganiserId != organiserId) return Forbid("You do not own this event.");

            existingEvent.Name = dto.Name;
            existingEvent.Description = dto.Description;
            existingEvent.EventDate = dto.EventDate;
            existingEvent.Location = dto.Location;
            existingEvent.DistanceKm = dto.DistanceKm;
            existingEvent.EventType = dto.EventType;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/events/5 (Only Organisers)
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null) return NotFound("Event not found.");

            var organiserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (existingEvent.OrganiserId != organiserId) return Forbid("You do not own this event.");

            _context.Events.Remove(existingEvent);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}