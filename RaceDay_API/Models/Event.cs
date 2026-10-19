using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        public int OrganiserId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        [Required, MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        public decimal DistanceKm { get; set; }

        public EventType EventType { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Scheduled;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("OrganiserId")]
        public User? Organiser { get; set; }
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Route> Routes { get; set; } = new List<Route>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
        public ICollection<WeatherCache> WeatherCaches { get; set; } = new List<WeatherCache>();
    }
}