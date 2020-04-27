using System;
using System.Collections.Generic;
using libAPICache.Models.Kimai;
using libKimai.models;

namespace libAPICache.Abstract
{
    public interface IKimaiTimeEntries
    {
        IEnumerable<TimeEntry> TimeEntries { get; }
        bool SaveEntry(Activity timeEntry);
        bool SaveEntry(TimeEntry timeEntry);
        bool SaveEntries(List<Activity> timeEntries);
        TimeEntry GetOrReturnNull(int id);
        void CacheEntries(DateTime? fromDate = null);
    }
}