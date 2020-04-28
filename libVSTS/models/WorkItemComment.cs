using System;

namespace libVSTS.models
{
    public class WorkItemComment
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comment { get; set; }
    }
}