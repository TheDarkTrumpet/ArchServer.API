using System;
using System.Collections.Generic;
using System.Linq;
using libAPICache.Abstract;
using libAPICache.Models.Toggl;
using libAPICache.util;
using libToggl.api;
using Microsoft.Extensions.Configuration;
using IConfiguration = libAPICache.util.IConfiguration;

namespace libAPICache.Entities
{
    public sealed class EFTogglTimeEntries : EFBase<TimeEntry, libToggl.models.TimeEntry>, ITogglTimeEntries
    {
        public EFTogglTimeEntries() : this(new EFDbContext(), new Configuration()) { }

        public EFTogglTimeEntries(EFDbContext context, IConfiguration configuration) : base(context, configuration)
        {
            Entries = DbSet = Context.TogglTimeEntries;
        }
        

        public void CacheEntries(string workspaceName, DateTime? fromDate = null)
        {
            string apiKey = GetAPIKey("APISources:Toggl:API_Key");
            TimeEntries activities = new TimeEntries(apiKey);

            List<libToggl.models.TimeEntry> results = activities.GetTimeEntries(workspaceName, fromDate).ToList();

            SaveEntries(results);
        }
    }
}