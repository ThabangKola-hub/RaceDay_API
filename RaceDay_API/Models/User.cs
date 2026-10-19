using RaceDay_API.Models;
using System.ComponentModel.DataAnnotations;

namespace RaceDay_API.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}
