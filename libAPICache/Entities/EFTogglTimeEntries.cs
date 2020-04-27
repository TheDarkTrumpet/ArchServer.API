using System;
using System.Collections.Generic;
using System.Linq;
using libAPICache.Abstract;
using libAPICache.Models.Toggl;
using libAPICache.util;
using libKimai.query;
using libToggl.api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace libAPICache.Entities
{
    public class EFTogglTimeEntries : EFBase<libAPICache.Models.Toggl.TimeEntry>
    {
        public IEnumerable<TimeEntry> TimeEntries => _context.TogglTimeEntries;

        public EFTogglTimeEntries()
        {
            Entries = _dbSet = _context.TogglTimeEntries;
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