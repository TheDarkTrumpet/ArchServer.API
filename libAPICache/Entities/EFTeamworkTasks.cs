using System;
using System.Collections.Generic;
using Configuration;
using libAPICache.Abstract;
using libTeamwork.api;

namespace libAPICache.Entities
{
    public sealed class EFTeamworkTasks : EFBase<Models.Teamwork.Task, libTeamwork.models.Task>, ITeamworkTasks
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        private ITasks _iTasks;
        
        public EFTeamworkTasks() : this(new EFDbContext(), new Config()) { }

        public EFTeamworkTasks(EFDbContext context, IConfig configuration, ITasks iTasks = null) : base(context, configuration)
        {
            Entries = DbSet = Context.TeamworkTasks;
            
            ApiKey = Configuration.GetKey("APISources:Teamwork:API_Key");
            BaseUrl = Configuration.GetKey("APISources:Teamwork:Base_URL");
            
            _iTasks = iTasks ?? new Tasks(ApiKey, BaseUrl);
        }
        
        public void CacheEntries(DateTime? fromDate = null, bool includeCompleted = true)
        {
            _iTasks.UpdatedAfterDate = fromDate;
            _iTasks.IncludeCompleted = includeCompleted;

            List<libTeamwork.models.Task> taskList = _iTasks.GetTasks();
            SaveEntries(taskList);
        }
    }
}