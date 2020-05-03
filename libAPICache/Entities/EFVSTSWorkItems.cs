using System;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using libAPICache.Abstract;
using libAPICache.Models.VSTS;
using libAPICache.util;
using libVSTS.api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace libAPICache.Entities
{
    public sealed class EFVSTSWorkItems: EFBase<Models.VSTS.WorkItem, libVSTS.models.WorkItem>, IVSTSWorkItems
    {
        public string ApiKey { get; set; }
        public string Organization { get; set; }
        public string Project { get; set; }
        
        private IWorkItem _workItem { get; set; }
        public EFVSTSWorkItems() : this(new EFDbContext(), new Config())
        {
        }

        public EFVSTSWorkItems(EFDbContext context, IConfig configuration, IWorkItem workItem = null) : base(context, configuration)
        {
            Entries = DbSet = Context.VSTSWorkItems;
            
            ApiKey = Configuration.GetKey("APISources:VSTS:API_Key");
            Organization = Configuration.GetKey("APISources:VSTS:Organization");
            Project = Configuration.GetKey("APISources:VSTS:Project");
            
            _workItem = workItem ?? new WorkItems(ApiKey, Organization, Project);
        }

        public void CacheEntries(bool includeComments = false, List<string> assignedToInclude = null,
            List<string> statesToExclude = null, List<string> typesToInclude = null, DateTime? fromChanged = null)
        {
            _workItem.FromChanged = fromChanged;
            _workItem.IncludeComments = includeComments;
            if (assignedToInclude != null && assignedToInclude.Any())
            {
                foreach (var ai in assignedToInclude)
                {
                    _workItem.AssignedToInclude.Add(ai);
                }
            }

            if (statesToExclude != null && statesToExclude.Any())
            {
                foreach (var si in statesToExclude)
                {
                    _workItem.StatesToExclude.Add(si);
                }
            }

            if (typesToInclude != null && typesToInclude.Any())
            {
                foreach (var ti in typesToInclude)
                {
                    _workItem.TypesToInclude.Add(ti);
                }
            }

            List<libVSTS.models.WorkItem> workItems = _workItem.GetWorkItems();
            SaveEntries(workItems);
        }

        // Comments are immutable, so this is simpler than it could have been. Check each end, verifying the elements exist, and add/remove as necessary
        // TODO Automapper may be good here, or a refactor of how the models and decouple from the API into their own set.
        public override WorkItem UpdateEnumerables(libVSTS.models.WorkItem source, Models.VSTS.WorkItem destination)
        {
            Context.Entry(destination).Collection(x => x.Comments).Load();
            
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
                    destination.Comments.Remove(wic);
                }
            }
            
            return destination;
        }
}
}