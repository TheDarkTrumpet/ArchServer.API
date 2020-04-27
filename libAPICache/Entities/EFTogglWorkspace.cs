using System.Collections.Generic;
using libAPICache.Abstract;
using libAPICache.Models.Toggl;
using libToggl.api;

namespace libAPICache.Entities
{
    public class EFTogglWorkspace : EFBase<Workspace, libToggl.models.Workspace>, ITogglWorkspace
    {
        public EFTogglWorkspace() : this(new EFDbContext()) { }
        public EFTogglWorkspace(EFDbContext context) : base(context) { }
        
        public void CacheEntries()
        {
            string apiKey = GetAPIKey("APISources:Toggl");
            Workspaces workspaces = new Workspaces(apiKey);

            List<libToggl.models.Workspace> workspaceList = workspaces.GetWorkspaces();
            SaveEntries(workspaceList);
        }
    }
}