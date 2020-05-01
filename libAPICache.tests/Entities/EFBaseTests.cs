using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using libAPICache.Entities;
using libAPICache.Models;
using libAPICache.Models.Kimai;
using libAPICache.tests.Helpers;
using libAPICache.util;
using libKimai.models;
using libVSTS.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkItemComment = libAPICache.Models.VSTS.WorkItemComment;

namespace libAPICache.tests.Entities
{
    
    [TestClass]
    public class EFBaseTests
    {
        private Mock<EFDbContext> _context;
        private BaseMock _baseMock;
        private Mock<DbSet<Models.VSTS.WorkItem>> _mockDbSet;
            
        [TestMethod]
        [ExpectedException(typeof(Exception), "This method must be implemented in the derived class!")]
        public void UpdateEnumerables_NoOverride_ShouldThrowException()
        {
            BaseMockWithoutEnumerables input = new BaseMockWithoutEnumerables();
            input.UpdateEnumerables(new WorkItem(), new Models.VSTS.WorkItem());
        }

        [TestMethod]
        public void EFBase_WithContext_ShouldSetIt()
        {
            Assert.IsNotNull(_baseMock.InsertedContext);
            Assert.AreSame(_context.Object, _baseMock.InsertedContext);
        }

        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(false, 0)]
        public void SaveEntry_WithObjectNonExistent_ShouldAddAndSave(bool saveChanges, int saveChangesExpected)
        {
            Models.VSTS.WorkItem input = GenerateModelWorkItem();

            _baseMock.SaveEntry(input, saveChanges);
            
            _context.Verify(x => x.SaveChanges(), Times.Exactly(saveChangesExpected));
            _mockDbSet.Verify(x => x.Add(It.IsAny<Models.VSTS.WorkItem>()), Times.Once);
            Assert.AreEqual(0, _baseMock.UpdateEntityDataTimesCalled);
            Assert.AreEqual(0, _baseMock.EnumerablesTimesCalled);
        }

        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(false, 0)]
        public void SaveEntry_WithObjectExistent_ShouldNotAddButMaybeSave(bool saveChanges, int saveChangesExpected)
        {
            Models.VSTS.WorkItem input = GenerateModelWorkItem();
            input.Id = 12345;
            
            Models.VSTS.WorkItem result = _baseMock.SaveEntry(input, saveChanges);
            
            _context.Verify(x => x.SaveChanges(), Times.Exactly(saveChangesExpected));
            _mockDbSet.Verify(x => x.Add(It.IsAny<Models.VSTS.WorkItem>()), Times.Never);
            Assert.AreEqual(1, _baseMock.UpdateEntityDataTimesCalled);
            Assert.AreEqual(1, _baseMock.EnumerablesTimesCalled);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(false, 0)]
        public void SaveEntry_WithAPIObject_ShouldCopyAndMaybeSave(bool saveChanges, int saveChangesExpected)
        {
            WorkItem input = GenerateVSTSAPIWorkItem();

            Models.VSTS.WorkItem result = _baseMock.SaveEntry(input, saveChanges);
            
            _context.Verify(x => x.SaveChanges(), Times.Exactly(saveChangesExpected));
            _mockDbSet.Verify(x => x.Add(It.IsAny<Models.VSTS.WorkItem>()), Times.Once);
            Assert.AreEqual(0, _baseMock.UpdateEntityDataTimesCalled);
            Assert.AreEqual(0, _baseMock.EnumerablesTimesCalled);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(false, 0)]
        public void SaveEntries_WithAPIObjects_ShouldInsertAndMaybeSave(bool saveChanges, int saveChangesExpected)
        {
            List<WorkItem> inputs = Enumerable.Range(1, 5).Select(x => GenerateVSTSAPIWorkItem()).ToList();

            List<Models.VSTS.WorkItem> results = _baseMock.SaveEntries(inputs, saveChanges);
            
            _context.Verify(x => x.SaveChanges(), Times.Exactly(saveChangesExpected));
            _mockDbSet.Verify(x => x.Add(It.IsAny<Models.VSTS.WorkItem>()), Times.Exactly(5));
            Assert.AreEqual(0, _baseMock.UpdateEntityDataTimesCalled);
            Assert.AreEqual(0, _baseMock.EnumerablesTimesCalled);
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count());
        }
        [TestMethod]
        public void GetOrReturnNull_WithElement_ShouldReturnIt()
        {
            Models.VSTS.WorkItem result = _baseMock.GetOrReturnNull(12345);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(12345, result.Id);
        }

        [TestMethod]
        public void GetOrReturnNull_WithNoElement_ShouldReturnNull()
        {
            Models.VSTS.WorkItem result = _baseMock.GetOrReturnNull(9999999);
            
            Assert.IsNull(result);
        }
        
        // SETUP and HELPERS
        
        [TestInitialize]
        public void Initialize()
        {
            IQueryable<Models.VSTS.WorkItem> workItems = new List<Models.VSTS.WorkItem>()
            {
                new Models.VSTS.WorkItem() {Id = 12345, Description = "Description #1"},
                new Models.VSTS.WorkItem() {Id = 54321, Description = "Description #2"}
            }.AsQueryable();
            
            _mockDbSet = GenerateDBSetHelper<Models.VSTS.WorkItem>.GenerateDbSet(workItems);
            _context = new Mock<EFDbContext>();
            
            _context.Setup(x => x.VSTSWorkItems).Returns(_mockDbSet.Object);
            _baseMock = new BaseMock(_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockDbSet = null;
            _context = null;
            _baseMock = null;
        }
 
        public WorkItem GenerateVSTSAPIWorkItem()
        {
            Fixture autoFixture = new Fixture();
            WorkItem workItem = autoFixture.Create<WorkItem>();
            return workItem;
        }

        public Models.VSTS.WorkItem GenerateModelWorkItem()
        {
            Fixture autoFixture = new Fixture();
            IEnumerable<WorkItemComment> comments =
                autoFixture.Build<WorkItemComment>().Without(x => x.WorkItem).CreateMany();
            Models.VSTS.WorkItem workItem = autoFixture.Build<Models.VSTS.WorkItem>()
                .With(x => x.Comments, comments.ToList())
                .Create();
            return workItem;
        }
    }
}