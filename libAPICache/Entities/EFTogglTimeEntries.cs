using System;
using System.Collections.Generic;
using System.Linq;
using libAPICache.Models.Toggl;
using libToggl.api;
using Microsoft.Extensions.Configuration;

namespace libAPICache.Entities
{
    public class EFTogglTimeEntries : EFBase<TimeEntry, libToggl.models.TimeEntry>
    {
        public EFTogglTimeEntries() : base()
        {
            _dbSet = _context.TogglTimeEntries;
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