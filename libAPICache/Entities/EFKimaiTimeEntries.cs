using System;
using System.Collections.Generic;
using System.Linq;
using libAPICache.Abstract;
using libAPICache.Models.Kimai;
using libAPICache.util;
using libKimai.models;
using libKimai.query;
using Microsoft.Extensions.Configuration;

namespace libAPICache.Entities
{
    public sealed class EFKimaiTimeEntries : EFBase<Models.Kimai.TimeEntry, libKimai.models.Activity>, IKimaiTimeEntries
    {
        public EFKimaiTimeEntries() : this(new EFDbContext()) { }

        public EFKimaiTimeEntries(EFDbContext context) : base(context)
        {
            Entries = _dbSet = _context.KimaiTimeEntries;
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