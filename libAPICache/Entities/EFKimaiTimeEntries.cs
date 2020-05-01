using System;
using System.Collections.Generic;
using Configuration;
using libAPICache.Abstract;
using libAPICache.util;
using libKimai.models;
using libKimai.query;

namespace libAPICache.Entities
{
    public sealed class EFKimaiTimeEntries : EFBase<Models.Kimai.TimeEntry, libKimai.models.Activity>, IKimaiTimeEntries
    {
        private IActivities _activities;
        public EFKimaiTimeEntries() : this(new EFDbContext(), new Config()) { }

        public EFKimaiTimeEntries(EFDbContext context, IConfig configuration, IActivities activities = null) : base(context, configuration)
        {
            Entries = DbSet = Context.KimaiTimeEntries;
            if (activities == null)
            {
                CreateActivities();
            }
            else
            {
                _activities = activities;
            }
        }
        
        public void CacheEntries(DateTime? fromDate = null)
        {
            _activities.FromDate = fromDate;
            List<Activity> results = _activities.GetActivities();

            SaveEntries(results);
        }

        private void CreateActivities()
        {
            string connectionString = Configuration.GetKey("APISources:Kimai:Mysql_CS");
            string timeZone = Configuration.GetKey("APISources.Kimai:TimeZone");
            _activities = new Activities(connectionString, timeZone);
        }
    }
}