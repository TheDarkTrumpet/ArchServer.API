using System;
using System.Collections.Generic;
using Configuration;
using libAPICache.Abstract;
using libTeamwork.api;

namespace libAPICache.Entities
{
    public sealed class EFTeamworkTasks : EFBase<Models.Teamwork.Task, libTeamwork.models.Task>, ITeamworkTasks
    {
        public EFTeamworkTasks() : this(new EFDbContext(), new Config()) { }

        public EFTeamworkTasks(EFDbContext context, IConfig configuration) : base(context, configuration)
        {
            Entries = DbSet = Context.TeamworkTasks;
        }
        
        public void CacheEntries(DateTime? fromDate = null, bool includeCompleted = true)
        {
            string apiKey = Configuration.GetKey("APISources:Teamwork:API_Key");
            string baseURL = Configuration.GetKey("APISources:Teamwork:Base_URL");
            Tasks tasks = new Tasks(apiKey, baseURL);
            tasks.UpdatedAfterDate = fromDate;
            tasks.IncludeCompleted = true;

            List<libTeamwork.models.Task> taskList = tasks.GetTasks();
            SaveEntries(taskList);
        }
    }
}