using System;
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
        private List<string> _assignedInclude { get; set; }
        private List<string> _statesExclude { get; set; }
        private List<string> _typesInclude { get; set; }
        private DateTime? _fromDate { get; set; }
        private bool _includeComments { get; set; }
        
        
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

        [TestMethod]
        [DataRow(false, false, false, false, null)]
        [DataRow(false, true, false, false, null)]
        [DataRow(false, false, true, false, null)]
        [DataRow(false, false, false, true, null)]
        [DataRow(false, false, false, false, "2020/01/01")]
        public void CacheEntries_WithVariousOptions_ShouldSetupPropertiesGetWorkItemsAndSave(bool includeComments, bool assignedInclude, bool statesExclude, bool typesInclude, string fromChangedString)
        {
            EFVSTSWorkItems efvstsWorkItems = new EFVSTSWorkItems(_context.Object, _config.Object, _iAPIMethod.Object);

            DateTime? fromChanged = null;
            if (!string.IsNullOrEmpty(fromChangedString))
            {
                fromChanged = DateTime.Parse(fromChangedString);
            }

            List<string> assignedToInclude = GenerateStringList(assignedInclude);
            List<string> statesToExclude = GenerateStringList(statesExclude);
            List<string> typesToInclude = GenerateStringList(typesInclude);
            
            efvstsWorkItems.CacheEntries(includeComments, assignedToInclude, 
                statesToExclude, typesToInclude, fromChanged);

            _iAPIMethod.Verify(x => x.GetWorkItems(), Times.Once);
            _context.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(includeComments, _includeComments);
            Assert.AreEqual(fromChanged, _fromDate);

            if (assignedInclude)
            {
                Assert.AreEqual(assignedToInclude, _assignedInclude);
            }
            else
            {
                Assert.IsNull(_assignedInclude);
            }
            
            if (statesExclude)
            {
                Assert.AreEqual(statesToExclude, _statesExclude);
            }
            else
            {
                Assert.IsNull(_statesExclude);
            }
            
            if (typesInclude)
            {
                Assert.AreEqual(typesToInclude, _typesInclude);
            }
            else
            {
                Assert.IsNull(_typesInclude);
            }
            
        }
        [TestInitialize]
        public void Initialize()
        {
            Setup();
            _config.Setup(x => x.GetKey("APISources:VSTS:API_Key")).Returns("An API Key");
            _config.Setup(x => x.GetKey("APISources:VSTS:Organization")).Returns("An Organization");
            _config.Setup(x => x.GetKey("APISources:VSTS:Project")).Returns("A Project");

            _iAPIMethod.Setup(x => x.GetWorkItems()).Returns(new List<WorkItem>());
            _iAPIMethod.SetupSet(x => x.AssignedToInclude).Callback(x => _assignedInclude = x);
            _iAPIMethod.SetupSet(x => x.StatesToExclude).Callback(x => _statesExclude = x);
            _iAPIMethod.SetupSet(x => x.TypesToInclude).Callback(x => _typesInclude = x);
            _iAPIMethod.SetupSet(x => x.FromChanged).Callback(x => _fromDate = x);
            _iAPIMethod.SetupSet(x => x.IncludeComments).Callback(x => _includeComments = x);
            
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

        private List<string> GenerateStringList(bool toCreate)
        {
            List<string> values = null;
            if (toCreate)
            {
                Fixture autoFixture = new Fixture();
                values = autoFixture.CreateMany<string>().ToList();
            }

            return values;
        }
    }
}