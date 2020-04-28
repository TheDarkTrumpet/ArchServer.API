using System;
using System.Collections.Generic;
using libAPICache.Models.VSTS;

namespace libAPICache.Abstract
{
    public interface IVSTSWorkItems : IBase<Models.VSTS.WorkItem, libVSTS.models.WorkItem>
    {
        void CacheEntries(bool includeComments = false, List<string> assignedToInclude = null,
            List<string> statesToExclude = null, List<string> typesToInclude = null, DateTime? fromChanged = null);

        WorkItem UpdateEnumerables(libVSTS.models.WorkItem source, Models.VSTS.WorkItem destination);
    }
}