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
    [Authorize] // Requires login
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/enrolments (Only Participants)
        [Authorize(Roles = "Participant")]
        [HttpPost]
        public async Task<ActionResult> CreateEnrolment(EnrolmentCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var category = await _context.Categories.Include(c => c.Event).FirstOrDefaultAsync(c => c.CategoryId == dto.CategoryId);
            if (category == null) return NotFound("Category not found.");

            if (category.EventId != dto.EventId)
                return BadRequest("The selected category does not belong to the selected event.");

            if (category.Event!.Status == EventStatus.Cancelled)
                return BadRequest("Cannot register for a cancelled event.");

            // Check for duplicate enrolment
            var exists = await _context.Enrolments.AnyAsync(e => e.UserId == userId && e.EventId == dto.EventId);
            if (exists) return BadRequest("You are already enrolled in this event.");

            // Check capacity
            var enrolledCount = await _context.Enrolments.CountAsync(e => e.CategoryId == dto.CategoryId && e.Status == EnrolmentStatus.Active);
            if (category.MaxParticipants > 0 && enrolledCount >= category.MaxParticipants)
                return BadRequest("This category is full.");

            var enrolment = new Enrolment
            {
                UserId = userId,
                EventId = dto.EventId,
                CategoryId = dto.CategoryId,
                Status = EnrolmentStatus.Active
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Successfully enrolled." });
        }

        // GET: api/enrolments/my-enrolments (Participants view their own)
        [HttpGet("my-enrolments")]
        public async Task<ActionResult<IEnumerable<EnrolmentResponseDto>>> GetMyEnrolments()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var enrolments = await _context.Enrolments
                .Include(e => e.Event)
                .Include(e => e.Category)
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return Ok(enrolments.Select(e => new EnrolmentResponseDto
            {
                EnrolmentId = e.EnrolmentId,
                EventId = e.EventId,
                EventName = e.Event!.Name,
                CategoryName = e.Category!.Name,
                EnrolmentDate = e.EnrolmentDate,
                Status = e.Status.ToString()
            }));
        }
    }
}