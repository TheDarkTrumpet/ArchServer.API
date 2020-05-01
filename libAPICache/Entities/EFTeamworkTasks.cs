using System;
using System.Collections.Generic;
using libAPICache.Abstract;
using libAPICache.Models.Teamwork;
using libTeamwork.api;

namespace libAPICache.Entities
{
    public sealed class EFTeamworkTasks : EFBase<Models.Teamwork.Task, libTeamwork.models.Task>, ITeamworkTasks
    {
        public EFTeamworkTasks() : this(new EFDbContext()) { }

        public EFTeamworkTasks(EFDbContext context) : base(context)
        {
            Entries = DbSet = Context.TeamworkTasks;
        }
        
        public void CacheEntries(DateTime? fromDate = null, bool includeCompleted = true)
        {
            string apiKey = GetAPIKey("APISources:Teamwork:API_Key");
            string baseURL = GetAPIKey("APISources:Teamwork:Base_URL");
            Tasks tasks = new Tasks(apiKey, baseURL);
            tasks.UpdatedAfterDate = fromDate;
            tasks.IncludeCompleted = true;

            List<libTeamwork.models.Task> taskList = tasks.GetTasks();
            SaveEntries(taskList);
        }
    }
}