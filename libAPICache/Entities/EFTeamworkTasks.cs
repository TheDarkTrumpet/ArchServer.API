using System;
using libAPICache.Abstract;

namespace libAPICache.Entities
{
    public class EFTeamworkTasks : EFBase<Models.Teamwork.Task, libTeamwork.models.Task>, ITeamworkTasks
    {
        public void CacheEntries(DateTime? fromDate)
        {
            
        }
    }
}