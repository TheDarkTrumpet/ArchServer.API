using System;
using System.Collections.Generic;
using System.Linq;
using libAPICache.Abstract;
using libAPICache.Models.Toggl;
using libAPICache.util;
using libKimai.query;
using libToggl.api;
using Microsoft.Extensions.Configuration;

namespace libAPICache.Entities
{
    public class EFTogglTimeEntries : ITogglTimeEntries
    {
        private readonly EFDbContext _context;
        public EFTogglTimeEntries() : this(new EFDbContext()) { }

        public EFTogglTimeEntries(EFDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<TimeEntry> TimeEntries => _context.TogglTimeEntries;
        
        public bool SaveEntry(libToggl.models.TimeEntry timeEntry)
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
                _context.TogglTimeEntries.Add(timeEntry);
            }

            _context.SaveChanges();
            return true;
        }

        public bool SaveEntries(List<libToggl.models.TimeEntry> timeEntries)
        {
            bool result = false;
            foreach (var te in timeEntries)
            {
                result = SaveEntry(te);
            }

            return result;
        }
        
        public TimeEntry GetOrReturnNull(long id)
        {
            return TimeEntries.FirstOrDefault(x => x.Id == id);
        }

        public void CacheEntries(string workspaceName, DateTime? fromDate = null)
        {
            IConfiguration config = util.Configuration.GetConfiguration();
            string apiKey = (string) config["APISources:Toggl"];

            if (String.IsNullOrEmpty(apiKey))
            {
                throw new Exception(
                    "Toggl API Key is not defined, please add to the appsettings!");
            }
            
            TimeEntries activities = new TimeEntries(apiKey);

            List<libToggl.models.TimeEntry> results = activities.GetTimeEntries(workspaceName, fromDate).ToList();

            SaveEntries(results);
        }
    }
}