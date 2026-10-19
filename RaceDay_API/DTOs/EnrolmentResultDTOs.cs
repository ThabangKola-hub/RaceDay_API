using System.ComponentModel.DataAnnotations;
using RaceDay_API.Models;

namespace RaceDay_API.DTOs
{
    public class EnrolmentCreateDto
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }

    public class EnrolmentResponseDto
    {
        public int EnrolmentId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime EnrolmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ResultCreateDto
    {
        [Required]
        public int EnrolmentId { get; set; }
        [Required]
        public TimeSpan FinishTime { get; set; }
        [Required]
        public int Position { get; set; }
    }

    public class ResultResponseDto
    {
        public int ResultId { get; set; }
        public int EnrolmentId { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
    }
}
// EnrolmentResultDTOs