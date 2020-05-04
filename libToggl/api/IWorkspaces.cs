using System.Collections.Generic;
using libToggl.models;
using Newtonsoft.Json.Linq;

namespace libToggl.api
{
    public interface IWorkspaces : IBase
    {
        public JArray GetRawWorkspaces();
        List<Workspace> GetWorkspaces();
        Workspace GetWorkspaceIdByName(string name);
    }
}