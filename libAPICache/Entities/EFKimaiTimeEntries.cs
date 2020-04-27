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
    public class EFKimaiTimeEntries : EFBase<Models.Kimai.TimeEntry, libKimai.models.Activity>
    {
        public EFKimaiTimeEntries() : this(new EFDbContext()) { }

        public EFKimaiTimeEntries(EFDbContext context) : base(context)
        {
            _dbSet = _context.KimaiTimeEntries;
        }
        
        public void CacheEntries(DateTime? fromDate = null)
        {
            IConfiguration config = util.Configuration.GetConfiguration();
            string connectionString = (string) config["APISources:Kimai"];

            if (String.IsNullOrEmpty(connectionString))
            {
                throw new Exception(
                    "MySQL Connection string was not found in the appsettings, please ensure it's there before running this");
            }
            
            Activities activities = new Activities(connectionString);

            activities.FromDate = fromDate;
            List<Activity> results = activities.GetActivities();

            SaveEntries(results);
        }
    }
}