using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using libAPICache.Entities;
using libVSTS.api;
using libVSTS.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libAPICache.tests.Entities
{
    [TestClass]
    public class EFVSTSWorkItemsTests : Base<Models.VSTS.WorkItem, IWorkItem>
    {
        [TestMethod]
        public void Constructor_ShouldSetPropertiesAndCallConfig()
        {
            EFVSTSWorkItems efvstsWorkItems = new EFVSTSWorkItems(_context.Object, _config.Object, _iAPIMethod.Object);
            
            _config.Verify(x => x.GetKey(It.IsAny<string>()), Times.Exactly(3));
            Assert.IsNotNull(efvstsWorkItems.Entries);
            Assert.IsNotNull(efvstsWorkItems.Organization);
            Assert.IsNotNull(efvstsWorkItems.Project);
            Assert.IsNotNull(efvstsWorkItems.ApiKey);
        }

        [TestInitialize]
        public void Initialize()
        {
            Setup();
            _config.Setup(x => x.GetKey("APISources:VSTS:API_Key")).Returns("An API Key");
            _config.Setup(x => x.GetKey("APISources:VSTS:Organization")).Returns("An Organization");
            _config.Setup(x => x.GetKey("APISources:VSTS:Project")).Returns("A Project");

            _iAPIMethod.Setup(x => x.GetWorkItems()).Returns(new List<WorkItem>());
            
            _context.Setup(x => x.VSTSWorkItems).Returns(_mockDbSet.Object);
        }

        protected override IQueryable<Models.VSTS.WorkItem> GenerateFixtures()
        {
            Fixture autoFixture = new Fixture();
            IEnumerable<Models.VSTS.WorkItemComment> workItemComments =
                autoFixture.Build<Models.VSTS.WorkItemComment>().Without(x => x.WorkItem).CreateMany();
            IQueryable<Models.VSTS.WorkItem> workItems = autoFixture.Build<Models.VSTS.WorkItem>()
                .Without(x => x.Comments)
                .Do(x => x.Comments = workItemComments.ToList()).CreateMany().AsQueryable();

            return workItems;
        }
    }
}