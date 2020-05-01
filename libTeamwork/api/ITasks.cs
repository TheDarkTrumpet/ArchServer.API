using System.Collections.Generic;
using libTeamwork.models;
using Newtonsoft.Json.Linq;

namespace libTeamwork.api
{
    public interface ITasks
    {
        JArray GetRawTasks();
        List<Task> GetTasks();
    }
}