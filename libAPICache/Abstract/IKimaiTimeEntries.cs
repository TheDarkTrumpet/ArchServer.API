using System;
using System.Collections.Generic;
using libAPICache.Models.Kimai;
using libKimai.models;

namespace libAPICache.Abstract
{
    public interface IKimaiTimeEntries : IBase<Models.Kimai.TimeEntry, libKimai.models.Activity>
    {
        void CacheEntries(DateTime? fromDate = null);
        string ConnectionString { get; set; }
        string TimeZone { get; set; }
    }
}