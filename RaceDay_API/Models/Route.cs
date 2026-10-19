using RaceDay_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public int EventId { get; set; }

        [Required, MaxLength(100)]
        public string RouteName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        public decimal DistanceKm { get; set; }

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}
