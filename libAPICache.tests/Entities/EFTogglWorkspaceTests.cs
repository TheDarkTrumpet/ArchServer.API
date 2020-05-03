using System.Collections.Generic;
using libAPICache.Entities;
using libToggl.api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libAPICache.tests.Entities
{
    [TestClass]
    public class EFTogglWorkspaceTests : Base<Models.Toggl.Workspace, IWorkspaces>
    {
        [TestMethod]
        public void Constructor_ShouldSetPropertiesAndCallConfig()
        {
            EFTogglWorkspace efTogglWorkspace = new EFTogglWorkspace(_context.Object, _config.Object, _iAPIMethod.Object);
            
            _config.Verify(x => x.GetKey(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(efTogglWorkspace.ApiKey);
            Assert.IsNotNull(efTogglWorkspace.Entries);
        }

        [TestMethod]
        public void CacheEntries_ShouldCallGetWorkspacesAndSave()
        {
            EFTogglWorkspace efTogglWorkspace = new EFTogglWorkspace(_context.Object, _config.Object, _iAPIMethod.Object);
            
            efTogglWorkspace.CacheEntries();
            
            _iAPIMethod.Verify(x => x.GetWorkspaces(), Times.Once);
            _context.Verify(x => x.SaveChanges(), Times.Once);
        }
        
        [TestInitialize]
        public void Initialize()
        {
            Setup();

            _config.Setup(x => x.GetKey("APISources:Toggl:API_Key")).Returns("Something");

            _iAPIMethod.Setup(x => x.GetWorkspaces()).Returns(new List<libToggl.models.Workspace>());

            _context.Setup(x => x.TogglWorkspaces).Returns(_mockDbSet.Object);
        }
    }
}