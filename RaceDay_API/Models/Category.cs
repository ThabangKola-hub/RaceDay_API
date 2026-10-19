using RaceDay_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int EventId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty; // e.g., "Under 20", "10km"

        [Column(TypeName = "decimal(5,2)")]
        public decimal DistanceKm { get; set; }

        public int MaxParticipants { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}
