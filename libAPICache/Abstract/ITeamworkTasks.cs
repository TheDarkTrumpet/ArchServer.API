using System;

namespace libAPICache.Abstract
{
    public interface ITeamworkTasks : IBase<Models.Teamwork.Task, libTeamwork.models.Task>
    {
        void CacheEntries(DateTime? fromDate = null, bool includeCompleted = true);
    }
}