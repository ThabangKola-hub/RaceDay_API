using System.ComponentModel.DataAnnotations;
using RaceDay_API.Models;

namespace RaceDay_API.DTOs
{
    public class EventCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        [Required, MaxLength(150)]
        public string Location { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public EventType EventType { get; set; }
    }

    public class EventResponseDto
    {
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int OrganiserId { get; set; }
    }
}
