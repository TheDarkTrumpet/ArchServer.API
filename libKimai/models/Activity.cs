using System;

namespace libKimai.models
{
    public class Activity
    {
        public int Id { get; set; }
        public string ActivityName { get; set; }
        public string ActivityComment { get; set; }
        public string ProjectName { get; set; }
        public string ProjectComment { get; set; }
        public string Customer { get; set; }
        public double? HourlyRate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TimeNotes { get; set; }
    }
}