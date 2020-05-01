using System.Collections.Generic;
using libAPICache.Abstract;
using libAPICache.Models.Toggl;
using libToggl.api;

namespace libAPICache.Entities
{
    public sealed class EFTogglWorkspace : EFBase<Workspace, libToggl.models.Workspace>, ITogglWorkspace
    {
        public EFTogglWorkspace() : this(new EFDbContext()) { }

        public EFTogglWorkspace(EFDbContext context) : base(context)
        {
            Entries = DbSet = Context.TogglWorkspaces;
        }
        
        public void CacheEntries()
        {
            string apiKey = GetAPIKey("APISources:Toggl:API_Key");
            Workspaces workspaces = new Workspaces(apiKey);

            List<libToggl.models.Workspace> workspaceList = workspaces.GetWorkspaces();
            SaveEntries(workspaceList);
        }
    }
}