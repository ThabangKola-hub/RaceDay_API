using RaceDay_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay_API.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int EnrolmentId { get; set; }

        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }

        [ForeignKey("EnrolmentId")]
        public Enrolment? Enrolment { get; set; }
    }
}