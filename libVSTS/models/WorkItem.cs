using System;
using System.Collections.Generic;

namespace libVSTS.models
{
    public class WorkItem
    {
        public WorkItem()
        {
            Comments = new List<WorkItemComment>();
        }
        public int id { get; set; }
        public string url { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public List<WorkItemComment> Comments { get; set; }
    }
}