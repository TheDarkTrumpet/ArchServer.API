using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libAPICache.Models.Kimai
{
    [Table(name: "TimeEntries", Schema = "Kimai")]
    public class TimeEntry : Base
    {
        [MaxLength(255)]
        public string ActivityName { get; set; }
        public string ActivityComment { get; set; }
        [MaxLength(255)]
        public string ProjectName { get; set; }
        public string ProjectComment { get; set; }
        [MaxLength(255)]
        public string Customer { get; set; }
        public double? HourlyRate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TimeNotes { get; set; }
    }
}