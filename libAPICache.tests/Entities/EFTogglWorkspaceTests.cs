using libToggl.api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libAPICache.tests.Entities
{
    public class EFTogglWorkspaceTests : Base<Models.Toggl.Workspace, IWorkspaces>
    {
        
        
        [TestInitialize]
        public void Initialize()
        {
            Setup();

            _config.Setup(x => x.GetKey("APISources:Toggl:API_Key")).Returns("Something");
            

        }
    }
}