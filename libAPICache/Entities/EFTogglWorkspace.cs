using System.Collections.Generic;
using libAPICache.Abstract;
using libAPICache.Models.Toggl;
using libAPICache.util;
using libToggl.api;

namespace libAPICache.Entities
{
    public sealed class EFTogglWorkspace : EFBase<Workspace, libToggl.models.Workspace>, ITogglWorkspace
    {
        public EFTogglWorkspace() : this(new EFDbContext(), new Configuration()) { }

        public EFTogglWorkspace(EFDbContext context, IConfiguration configuration) : base(context, configuration)
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