using System.Collections.Generic;
using Configuration;
using libAPICache.Abstract;
using libAPICache.Models.Toggl;
using libAPICache.util;
using libToggl.api;

namespace libAPICache.Entities
{
    public sealed class EFTogglWorkspace : EFBase<Workspace, libToggl.models.Workspace>, ITogglWorkspace
    {
        private IWorkspaces _workspaces;
        public string ApiKey;
        
        public EFTogglWorkspace() : this(new EFDbContext(), new Config()) { }

        public EFTogglWorkspace(EFDbContext context, IConfig configuration, IWorkspaces workspaces = null) : base(context, configuration)
        {
            Entries = DbSet = Context.TogglWorkspaces;

            ApiKey = Configuration.GetKey("APISources:Toggl:API_Key");
            _workspaces = workspaces ?? new Workspaces(ApiKey);
        }
        
        public void CacheEntries()
        {
            _workspaces.GenerateClient();
            List<libToggl.models.Workspace> workspaceList = _workspaces.GetWorkspaces();
            SaveEntries(workspaceList);
        }
    }
}



