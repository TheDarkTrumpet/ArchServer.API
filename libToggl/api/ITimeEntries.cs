using System;
using System.Collections.Generic;
using libToggl.models;
using Newtonsoft.Json.Linq;

namespace libToggl.api
{
    public interface ITimeEntries : IBase
    {
        JArray GetRawTimeEntries(string workspaceName, DateTime? startDate = null, DateTime? endDate = null);

        JArray GetRawTimeEntries(Workspace workspace, DateTime? startDate = null, DateTime? endDate = null,
            int page = 1);

        IEnumerable<TimeEntry> GetTimeEntries(Workspace workspace, DateTime? startDate = null,
            DateTime? endDate = null);

        IEnumerable<TimeEntry> GetTimeEntries(string name, DateTime? startDate = null, DateTime? endDate = null);
        
    }
}