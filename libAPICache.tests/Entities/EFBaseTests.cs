using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

namespace libAPICache.tests.Entities
{
    
    [TestClass]
    public class EFBaseTests
    {
        private Mock<EFDbContext> _context;
        private BaseMock _baseMock;
        private Mock<DbSet<Models.VSTS.WorkItem>> _mockDbSet;
            
        [TestMethod]
        [ExpectedException(typeof(Exception))]
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
        public void SaveEntry_WithObjectNonExistent_ShouldAddAndSave()
        {
            WorkItem input = new WorkItem()
            {
                Id = 11111,
                Description= "New Description"
            };

            _baseMock.SaveEntry(input);
            
            _context.Verify(x => x.SaveChanges(), Times.Once);
            _mockDbSet.Verify(x => x.Add(It.IsAny<Models.VSTS.WorkItem>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntry_WithObjectExistent_ShouldNotAddButSave()
        {
            WorkItem input = new WorkItem()
            {
                Id = 12345,
                Description = "A New comment"
            };

            Models.VSTS.WorkItem result = _baseMock.SaveEntry(input);
            
            _context.Verify(x => x.SaveChanges(), Times.Once);
            _mockDbSet.Verify(x => x.Add(It.IsAny<Models.VSTS.WorkItem>()), Times.Never);
            Assert.AreEqual(1, _baseMock.UpdateEntityDataTimesCalled);
            Assert.AreEqual(1, _baseMock.EnumerablesTimesCalled);
            Assert.IsNotNull(result);
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
        
        private class BaseMockWithoutEnumerables : EFBase<Models.VSTS.WorkItem, libVSTS.models.WorkItem>
        {
            //Empty!
        }

        private class BaseMock : EFBase<Models.VSTS.WorkItem, libVSTS.models.WorkItem>
        {
            public int EnumerablesTimesCalled { get; set; } = 0;
            public int UpdateEntityDataTimesCalled { get; set; } = 0;
            public EFDbContext InsertedContext { get; set; }

            public BaseMock(EFDbContext context) : base(context)
            {
                InsertedContext = context;
                Entries = _dbSet = context.VSTSWorkItems;
            }

            public override Models.VSTS.WorkItem UpdateEnumerables(WorkItem activity, Models.VSTS.WorkItem timeEntry)
            {
                EnumerablesTimesCalled += 1;
                return timeEntry;
            }

            public override void UpdateEntityData(Models.VSTS.WorkItem destination, Models.VSTS.WorkItem workItem)
            {
                UpdateEntityDataTimesCalled += 1;
            }
        }
    }
}