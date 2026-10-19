using RaceDay_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Enrolment
    {
        public int EnrolmentId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int CategoryId { get; set; }
        public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;
        public EnrolmentStatus Status { get; set; } = EnrolmentStatus.Active;

        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("EventId")]
        public Event? Event { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public Result? Result { get; set; }
    }
}
//Add Enrolment model