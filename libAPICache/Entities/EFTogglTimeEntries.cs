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
        public string ApiKey { get; set; }
        public ITimeEntries _timeEntries { get; set; }
        public EFTogglTimeEntries() : this(new EFDbContext(), new Config(), null) { }

        public EFTogglTimeEntries(EFDbContext context, IConfig configuration, ITimeEntries timeEntries) : base(context, configuration)
        {
            Entries = DbSet = Context.TogglTimeEntries;
            
            ApiKey = Configuration.GetKey("APISources:Toggl:API_Key");
            _timeEntries = timeEntries ?? new TimeEntries(ApiKey);
        }
        

        public void CacheEntries(string workspaceName, DateTime? fromDate = null)
        {
            _timeEntries.GenerateClient();
            List<libToggl.models.TimeEntry> results = _timeEntries.GetTimeEntries(workspaceName, fromDate).ToList();

            SaveEntries(results);
        }
    }
}