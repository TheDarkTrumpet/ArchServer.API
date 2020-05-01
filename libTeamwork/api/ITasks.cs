using System;
using System.Collections.Generic;
using libTeamwork.models;
using Newtonsoft.Json.Linq;

namespace libTeamwork.api
{
    public interface ITasks
    {
        bool IncludeCompleted { get; set; }
        DateTime? UpdatedAfterDate { get; set; }
        
        JArray GetRawTasks();
        List<Task> GetTasks();
    }
}