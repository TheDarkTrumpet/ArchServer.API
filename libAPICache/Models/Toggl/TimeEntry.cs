using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libAPICache.Models.Toggl
{
    [Table(name: "TimeEntries", Schema = "Toggl")]
    public class TimeEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        [MaxLength(50)]
        public string User { get; set; }
        [MaxLength(100)]
        public string Client { get; set; }
        [MaxLength(100)]
        public string Project { get; set; }
        public double Billable { get; set; }
        public bool IsBillable { get; set; }   
    }
}