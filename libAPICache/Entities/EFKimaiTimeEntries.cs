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
        public string ConnectionString { get; set; }
        public string TimeZone { get; set; }
        
        public EFKimaiTimeEntries() : this(new EFDbContext(), new Config()) { }

        public EFKimaiTimeEntries(EFDbContext context, IConfig configuration, IActivities activities = null) : base(context, configuration)
        {
            Entries = DbSet = Context.KimaiTimeEntries;
            
            ConnectionString = Configuration.GetKey("APISources:Kimai:Mysql_CS");
            TimeZone = Configuration.GetKey("APISources.Kimai:TimeZone");
            
            _activities = activities ?? new Activities(ConnectionString, TimeZone);
        }
        
        public void CacheEntries(DateTime? fromDate = null)
        {
            _activities.FromDate = fromDate;
            List<Activity> results = _activities.GetActivities();

            SaveEntries(results);
        }
    }
}