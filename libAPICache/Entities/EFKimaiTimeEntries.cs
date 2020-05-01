using System;
using System.Collections.Generic;
using libAPICache.Abstract;
using libAPICache.util;
using libKimai.models;
using libKimai.query;

namespace libAPICache.Entities
{
    public sealed class EFKimaiTimeEntries : EFBase<Models.Kimai.TimeEntry, libKimai.models.Activity>, IKimaiTimeEntries
    {
        public EFKimaiTimeEntries() : this(new EFDbContext(), new util.Configuration()) { }

        public EFKimaiTimeEntries(EFDbContext context, Configuration configuration) : base(context, configuration)
        {
            Entries = DbSet = Context.KimaiTimeEntries;
        }
        
        public void CacheEntries(DateTime? fromDate = null, string timeZone = "Central Standard Time")
        {
            string connectionString = GetAPIKey("APISources:Kimai:Mysql_CS");
            
            Activities activities = new Activities(connectionString, timeZone);

            activities.FromDate = fromDate;
            List<Activity> results = activities.GetActivities();

            SaveEntries(results);
        }
    }
}