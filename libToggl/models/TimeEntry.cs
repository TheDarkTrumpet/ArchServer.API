using System;

namespace libToggl.models
{
    public class TimeEntry
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string User { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public double Billable { get; set; }
        public bool IsBillable { get; set; }
     }
}