using System.ComponentModel.DataAnnotations;

namespace RaceDay_API.DTOs
{
    public class CategoryDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public int MaxParticipants { get; set; }
    }

    public class RouteDto
    {
        [Required, MaxLength(100)]
        public string RouteName { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
//CategoryRouteDTO