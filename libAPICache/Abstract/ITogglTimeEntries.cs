using System;
using System.Collections.Generic;
using libAPICache.Models.Toggl;

namespace libAPICache.Abstract
{
    public interface ITogglTimeEntries
    {
        IEnumerable<TimeEntry> TimeEntries { get; }
        bool SaveEntry(libToggl.models.TimeEntry timeEntry);
        bool SaveEntry(TimeEntry timeEntry);
        bool SaveEntries(List<libToggl.models.TimeEntry> timeEntries);
        TimeEntry GetOrReturnNull(long id);
        void CacheEntries(string workspaceName, DateTime? fromDate = null);
    }
}