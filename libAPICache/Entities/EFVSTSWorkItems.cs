using System;
using System.Collections.Generic;
using System.Linq;
using libAPICache.Models.VSTS;
using libAPICache.util;
using libVSTS.api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            Console.WriteLine("Include Comments: " + workItemQuery.IncludeComments);
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

        // Comments are immutable, so this is simpler than it could have been. Check each end, verifying the elements exist, and add/remove as necessary
        // TODO Automapper may be good here, or a refactor of how the models and decouple from the API into their own set.
        public override WorkItem UpdateEnumerables(libVSTS.models.WorkItem source, Models.VSTS.WorkItem destination)
        {
            foreach (libVSTS.models.WorkItemComment wic in source.Comments)
            {
                if (destination.Comments.All(x => x.Id != wic.Id))
                {
                    WorkItemComment newComment = new WorkItemComment();
                    newComment.Copy(wic);
                    destination.Comments.Add(newComment);
                }
            }

            foreach (WorkItemComment wic in destination.Comments)
            {
                if (source.Comments.All(x => x.Id != wic.Id))
                {
                    Console.WriteLine("removing elements");
                    destination.Comments.Remove(wic);
                }
            }
            
            return destination;
        }
}
}