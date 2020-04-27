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
    public class EFKimaiTimeEntries : IKimaiTimeEntries
    {
        private readonly EFDbContext _context;
        public EFKimaiTimeEntries() : this(new EFDbContext()) { }

        public EFKimaiTimeEntries(EFDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TimeEntry> TimeEntries => _context.KimaiTimeEntries;

        public bool SaveEntry(Activity timeEntry)
        {
            TimeEntry saveTimeEntry = new TimeEntry();

            saveTimeEntry.Copy(timeEntry);
            SaveEntry(saveTimeEntry);

            return true;
        }

        public bool SaveEntry(TimeEntry timeEntry)
        {
            TimeEntry srcEntry = GetOrReturnNull(timeEntry.Id);

            if (srcEntry != null)
            {
                _context.Entry(srcEntry).CurrentValues.SetValues(timeEntry);
            }
            else
            {
                _context.KimaiTimeEntries.Add(timeEntry);
            }

            _context.SaveChanges();
            return true;
        }

        public bool SaveEntries(List<Activity> timeEntries)
        {
            bool result = false;
            foreach (var te in timeEntries)
            {
                result = SaveEntry(te);
            }

            return result;
        }
        
        public TimeEntry GetOrReturnNull(int id)
        {
            return TimeEntries.FirstOrDefault(x => x.Id == id);
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
        }
    }
}