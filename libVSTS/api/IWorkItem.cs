using System;
using System.Collections.Generic;
using libVSTS.models;
using Newtonsoft.Json.Linq;

namespace libVSTS.api
{
    public interface IWorkItem
    {
        List<string> StatesToExclude { get; set; }
        List<string> AssignedToInclude { get; set; }
        List<string> TypesToInclude { get; set; }
        DateTime? FromChanged { get; set; }
        Boolean IncludeComments { get; set; }

        JArray GetRawWorkItems();
        List<WorkItem> GetWorkItems();
        string BuildQuery();
        
    }
}