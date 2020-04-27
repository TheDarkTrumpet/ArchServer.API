using System;
using System.Collections.Generic;
using libAPICache.Abstract;
using libAPICache.Models.Teamwork;
using libTeamwork.api;

namespace libAPICache.Entities
{
    public class EFTeamworkTasks : EFBase<Models.Teamwork.Task, libTeamwork.models.Task>, ITeamworkTasks
    {
        public EFTeamworkTasks(EFDbContext context) : base(context)
        {
            Entries = _dbSet = _context.TeamworkTasks;
        }
        
        public void CacheEntries(DateTime? fromDate = null, bool includeCompleted = true)
        {
            string apiKey = GetAPIKey("APISources:Teamwork");
            string baseURL = GetAPIKey("APISources:TeamworkURL");
            Tasks tasks = new Tasks(apiKey, baseURL);
            tasks.UpdatedAfterDate = fromDate;
            tasks.IncludeCompleted = true;

            List<libTeamwork.models.Task> taskList = tasks.GetTasks();
            SaveEntries(taskList);
        }
    }
}