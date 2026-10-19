using RaceDay_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class WeatherCache
    {
        [Key]
        public int WeatherId { get; set; }
        public int EventId { get; set; }

        public double TemperatureC { get; set; }

        [MaxLength(100)]
        public string Conditions { get; set; } = string.Empty;

        public double WindSpeedKmh { get; set; }
        public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}