using System.Data.Entity;
using libAPICache.Models;

namespace libAPICache.Entities
{
    public class EFDbContext : DbContext
    {
        public DbSet<Models.Kimai.TimeEntry> KimaiTimeEntries {get;set;}
        public DbSet<Models.Teamwork.Person> TeamworkPeople { get; set; }
        public DbSet<Models.Teamwork.Task> TeamworkTasks { get; set; }
        public DbSet<Models.Toggl.TimeEntry> TogglTimeEntries { get; set; }
        public DbSet<Models.VSTS.WorkItem> VSTSWorkItems { get; set; }
        public DbSet<Models.VSTS.WorkItemComment> VSTSWorkItemComments { get; set; }
    }
}