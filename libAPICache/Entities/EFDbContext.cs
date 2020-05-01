using libAPICache.Models;
using libAPICache.util;
using Microsoft.EntityFrameworkCore;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace libAPICache.Entities
{
    public class EFDbContext : DbContext
    {
        public virtual DbSet<Models.Kimai.TimeEntry> KimaiTimeEntries {get;set;}
        public virtual DbSet<Models.Teamwork.Person> TeamworkPeople { get; set; }
        public virtual DbSet<Models.Teamwork.Task> TeamworkTasks { get; set; }
        public virtual DbSet<Models.Toggl.TimeEntry> TogglTimeEntries { get; set; }
        public virtual DbSet<Models.Toggl.Workspace> TogglWorkspaces { get; set; }
        public virtual DbSet<Models.VSTS.WorkItem> VSTSWorkItems { get; set; }
        public virtual DbSet<Models.VSTS.WorkItemComment> VSTSWorkItemComments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Configuration configuration = new Configuration();
            string connectionString = configuration.GetKey("ConnectionStrings:EFDbContext");

            optionsBuilder.UseSqlServer(connectionString);
        }
        
        public EFDbContext(): base() { }
    }
}