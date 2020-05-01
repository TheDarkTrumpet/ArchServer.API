using System;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using libAPICache.Abstract;
using libAPICache.Models.Toggl;
using libToggl.api;

namespace libAPICache.Entities
{
    public sealed class EFTogglTimeEntries : EFBase<TimeEntry, libToggl.models.TimeEntry>, ITogglTimeEntries
    {
        public EFTogglTimeEntries() : this(new EFDbContext(), new Config()) { }

        public EFTogglTimeEntries(EFDbContext context, IConfig configuration) : base(context, configuration)
        {
            Entries = DbSet = Context.TogglTimeEntries;
        }
        

        public void CacheEntries(string workspaceName, DateTime? fromDate = null)
        {
            string apiKey = Configuration.GetKey("APISources:Toggl:API_Key");
            TimeEntries activities = new TimeEntries(apiKey);

            List<libToggl.models.TimeEntry> results = activities.GetTimeEntries(workspaceName, fromDate).ToList();

            SaveEntries(results);
        }
    }
}