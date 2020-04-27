using System;
using System.Collections.Generic;
using System.Linq;
using libAPICache.Models.VSTS;
using libVSTS.api;
using Microsoft.EntityFrameworkCore;

namespace libAPICache.Entities
{
    public class EFVSTSWorkItems : EFBase<Models.VSTS.WorkItem, libVSTS.models.WorkItem>
    {
        public EFVSTSWorkItems() : this(new EFDbContext())
        {
        }

        public EFVSTSWorkItems(EFDbContext context) : base(context)
        {
            Entries = _dbSet = _context.VSTSWorkItems;
        }

        public void CacheEntries(bool includeComments = false, List<string> assignedToInclude = null,
            List<string> statesToExclude = null, List<string> typesToInclude = null, DateTime? fromChanged = null)
        {
            string api_key = GetAPIKey("APISources:VSTS:API_Key");
            string organization = GetAPIKey("APISources:VSTS:Organization");
            string project = GetAPIKey("APISources:VSTS:Project");

            WorkItems workItemQuery = new WorkItems(api_key, organization, project);
            workItemQuery.FromChanged = fromChanged;
            workItemQuery.IncludeComments = includeComments;

            if (assignedToInclude != null && assignedToInclude.Any())
            {
                foreach (var ai in assignedToInclude)
                {
                    workItemQuery.AssignedToInclude.Add(ai);
                }
            }

            if (statesToExclude != null && statesToExclude.Any())
            {
                foreach (var si in statesToExclude)
                {
                    workItemQuery.StatesToExclude.Add(si);
                }
            }

            if (typesToInclude != null && typesToInclude.Any())
            {
                foreach (var ti in typesToInclude)
                {
                    workItemQuery.TypesToInclude.Add(ti);
                }
            }

            List<libVSTS.models.WorkItem> workItems = workItemQuery.GetWorkItems();
            SaveEntries(workItems);
        }

        public override WorkItem UpdateEnumerables(Models.VSTS.WorkItem source, Models.VSTS.WorkItem destination)
        {
            return destination;
        }
}
}