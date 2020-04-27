using Microsoft.Extensions.Configuration;
using libAPICache.Models;
using Microsoft.EntityFrameworkCore;

namespace libAPICache.Entities
{
    public class EFDbContext : DbContext
    {
        public DbSet<Models.Kimai.TimeEntry> KimaiTimeEntries {get;set;}
        public DbSet<Models.Teamwork.Person> TeamworkPeople { get; set; }
        public DbSet<Models.Teamwork.Task> TeamworkTasks { get; set; }
        public DbSet<Models.Toggl.TimeEntry> TogglTimeEntries { get; set; }
        public DbSet<Models.Toggl.Workspace> TogglWorkspaces { get; set; }
        public DbSet<Models.VSTS.WorkItem> VSTSWorkItems { get; set; }
        public DbSet<Models.VSTS.WorkItemComment> VSTSWorkItemComments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = util.Configuration.GetConfiguration();
            string connectionString = (string) config["ConnectionStrings:EFDbContext"];

            optionsBuilder.UseSqlServer(connectionString);
        }
        
        public EFDbContext(): base() { }
    }
}