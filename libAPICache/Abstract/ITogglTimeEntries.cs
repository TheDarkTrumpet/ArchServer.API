using System;
using System.Collections.Generic;
using libAPICache.Models.Toggl;

namespace libAPICache.Abstract
{
    public interface ITogglTimeEntries : IBase<Models.Toggl.TimeEntry, libToggl.models.TimeEntry>
    {
        void CacheEntries(string workspaceName, DateTime? fromDate = null);
        void CacheEntries(string workspaceName, int? fromDateDays = null);
    }
}