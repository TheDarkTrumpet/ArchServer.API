using System;

namespace libTeamwork.models
{
    public class Task
    {
        public long Id { get; set; }
        public string ProjectName { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public Boolean Completed { get; set; }
    }
}