using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libAPICache.Models.VSTS
{
    [Table(name: "WorkItems", Schema = "VSTS")]
    public class WorkItem
    {
        public WorkItem()
        {
            Comments = new List<WorkItemComment>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        [MaxLength(255)]
        public string url { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public string Description { get; set; }
        [MaxLength(100)]
        public string AssignedTo { get; set; }
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public List<WorkItemComment> Comments { get; set; }
    }
}