using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Configuration;
using libAPICache.Entities;
using libVSTS.api;
using libVSTS.models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkItemComment = libAPICache.Models.VSTS.WorkItemComment;

namespace libAPICache.tests.Entities
{
    [TestClass]
    public class EFVSTSWorkItemsTests : Base<Models.VSTS.WorkItem, IWorkItem>
    {
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
        [DataRow(true, false, false, false, null)]
        [DataRow(false, true, false, false, null)]
        [DataRow(false, false, true, false, null)]
        [DataRow(false, false, false, true, null)]
        [DataRow(false, false, false, false, "2020/01/01")]
        [DataRow(true, true, true, true, "2020/01/01")]
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
            CollectionAssert.AreEqual(assignedToInclude, _iAPIMethod.Object.AssignedToInclude);
            CollectionAssert.AreEqual(statesToExclude, _iAPIMethod.Object.StatesToExclude);
            CollectionAssert.AreEqual(typesToInclude, _iAPIMethod.Object.TypesToInclude);
        }

        [TestMethod]
        public void UpdateEnumerables_ShouldSynchronizeChildren()
        {
            EFVSTSWorkItemsMock efvstsWorkItems = new EFVSTSWorkItemsMock(_context.Object, _config.Object, _iAPIMethod.Object);

            libVSTS.models.WorkItem source = new WorkItem()
            {
                Comments = new List<libVSTS.models.WorkItemComment>()
                {
                    new libVSTS.models.WorkItemComment() { Id = 1, Comment = "Id 1"},
                    new libVSTS.models.WorkItemComment() { Id = 3, Comment = "Id 3"}
                }
            };

            Models.VSTS.WorkItem destination = new Models.VSTS.WorkItem()
            {
                Comments = new List<WorkItemComment>()
                {
                    new WorkItemComment() {Id = 1, Comment = "Id 1"},
                    new WorkItemComment() {Id = 2, Comment = "Id 2"}
                }
            };

            Models.VSTS.WorkItem result = efvstsWorkItems.UpdateEnumerables(source, destination);
            List<long> expected = new List<long>() { 1, 3 };
            
            Assert.AreEqual(2, result.Comments.Count);
            Assert.AreEqual(1, efvstsWorkItems.TimesCalled);
            CollectionAssert.AreEqual(expected, result.Comments.Select(x => x.Id).ToList());
        }
        
        [TestInitialize]
        public void Initialize()
        {
            Setup();
            _config.Setup(x => x.GetKey("APISources:VSTS:API_Key")).Returns("An API Key");
            _config.Setup(x => x.GetKey("APISources:VSTS:Organization")).Returns("An Organization");
            _config.Setup(x => x.GetKey("APISources:VSTS:Project")).Returns("A Project");

            _iAPIMethod.Setup(x => x.GetWorkItems()).Returns(new List<WorkItem>());
            _iAPIMethod.SetupAllProperties();
            _iAPIMethod.Object.AssignedToInclude = new List<string>();
            _iAPIMethod.Object.StatesToExclude = new List<string>();
            _iAPIMethod.Object.TypesToInclude = new List<string>();

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
            List<string> values = new List<string>();
            if (toCreate)
            {
                Fixture autoFixture = new Fixture();
                values = autoFixture.CreateMany<string>().ToList();
            }

            return values;
        }

        private class EFVSTSWorkItemsMock : EFVSTSWorkItems
        {
            public int TimesCalled { get; set; } = 0;
            public EFVSTSWorkItemsMock(EFDbContext context, IConfig configuration, IWorkItem workItem = null) : base(
                context, configuration, workItem)
            {
                
            }

            protected override void LoadCommentsOnObject(Models.VSTS.WorkItem destination)
            {
                TimesCalled += 1;
            }
        }
    }
}