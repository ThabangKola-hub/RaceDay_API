using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay_API.Data;
using RaceDay_API.DTOs;
using RaceDay_API.Models;

namespace RaceDay_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/results (Only Organisers)
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<ActionResult> CaptureResult(ResultCreateDto dto)
        {
            var enrolment = await _context.Enrolments.FindAsync(dto.EnrolmentId);
            if (enrolment == null) return NotFound("Enrolment not found.");

            var resultExists = await _context.Results.AnyAsync(r => r.EnrolmentId == dto.EnrolmentId);
            if (resultExists) return BadRequest("A result has already been recorded for this enrolment.");

            var result = new Result
            {
                EnrolmentId = dto.EnrolmentId,
                FinishTime = dto.FinishTime,
                Position = dto.Position
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Result recorded successfully." });
        }

        // GET: api/results (Only Organisers)
        [Authorize(Roles = "Organiser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultResponseDto>>> GetAllResults()
        {
            var results = await _context.Results
                .Include(r => r.Enrolment).ThenInclude(e => e!.User)
                .ToListAsync();

            return Ok(results.Select(r => new ResultResponseDto
            {
                ResultId = r.ResultId,
                EnrolmentId = r.EnrolmentId,
                ParticipantName = r.Enrolment!.User!.Name,
                FinishTime = r.FinishTime,
                Position = r.Position
            }));
        }
    }
}