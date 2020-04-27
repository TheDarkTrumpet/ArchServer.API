using System;
using System.Collections.Generic;
using libAPICache.Abstract;
using libAPICache.Models.Teamwork;
using libTeamwork.api;

namespace libAPICache.Entities
{
    public class EFTeamworkTasks : EFBase<Models.Teamwork.Task, libTeamwork.models.Task>, ITeamworkTasks
    {
        private readonly string _baseURL;
        public EFTeamworkTasks(string baseUrl) : this(new EFDbContext(), baseUrl) { }

        public EFTeamworkTasks(EFDbContext context, string baseUrl) : base(context)
        {
            _baseURL = baseUrl;
            Entries = _dbSet = _context.TeamworkTasks;
        }
        
        public void CacheEntries(DateTime? fromDate = null, bool includeCompleted = true)
        {
            string apiKey = GetAPIKey("APISources:Teamwork");
            Tasks tasks = new Tasks(apiKey, _baseURL);
            tasks.UpdatedAfterDate = fromDate;
            tasks.IncludeCompleted = true;

            List<libTeamwork.models.Task> taskList = tasks.GetTasks();
            SaveEntries(taskList);
        }
    }
}