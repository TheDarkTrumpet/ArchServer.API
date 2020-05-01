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
        private Mock<DbSet<TimeEntry>> _mockDbSet;
            
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateEnumerables_NoOverride_ShouldThrowException()
        {
            BaseMockWithoutEnumerables input = new BaseMockWithoutEnumerables();
            input.UpdateEnumerables(new Activity(), new TimeEntry());
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
            TimeEntry input = new TimeEntry()
            {
                Id = 11111,
                ProjectName = "New Project Name"
            };

            _baseMock.SaveEntry(input);
            
            _context.Verify(x => x.SaveChanges(), Times.Once);
            _mockDbSet.Verify(x => x.Add(It.IsAny<TimeEntry>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntry_WithObjectExistent_ShouldNotAddButSave()
        {
            TimeEntry input = new TimeEntry()
            {
                Id = 12345,
                ActivityComment = "A New comment"
            };

            TimeEntry result = _baseMock.SaveEntry(input);
            
            _context.Verify(x => x.SaveChanges(), Times.Once);
            _mockDbSet.Verify(x => x.Add(It.IsAny<TimeEntry>()), Times.Never);
            Assert.AreEqual(1, _baseMock.UpdateEntityDataTimesCalled);
        }
        
        [TestInitialize]
        public void Initialize()
        {
            IQueryable<TimeEntry> timeEntries = new List<TimeEntry>()
            {
                new TimeEntry() {Id = 12345, ActivityComment = "A Comment"},
                new TimeEntry() {Id = 54321, ActivityName = "A Name"}
            }.AsQueryable();
            
            _mockDbSet = GenerateDBSetHelper<TimeEntry>.GenerateDbSet(timeEntries);
            _context = new Mock<EFDbContext>();
            
            _context.Setup(x => x.KimaiTimeEntries).Returns(_mockDbSet.Object);
            _baseMock = new BaseMock(_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockDbSet = null;
            _context = null;
            _baseMock = null;
        }
        
        private class BaseMockWithoutEnumerables : EFBase<Models.Kimai.TimeEntry, libKimai.models.Activity>
        {
            //Empty!
        }

        private class BaseMock : EFBase<Models.Kimai.TimeEntry, libKimai.models.Activity>
        {
            public int EnumerablesTimesCalled { get; set; } = 0;
            public int UpdateEntityDataTimesCalled { get; set; } = 0;
            public EFDbContext InsertedContext { get; set; }

            public BaseMock(EFDbContext context) : base(context)
            {
                InsertedContext = context;
                Entries = _dbSet = context.KimaiTimeEntries;
            }

            public override TimeEntry UpdateEnumerables(Activity activity, TimeEntry timeEntry)
            {
                EnumerablesTimesCalled += 1;
                return timeEntry;
            }

            public override void UpdateEntityData(TimeEntry destination, TimeEntry activity)
            {
                UpdateEntityDataTimesCalled += 1;
            }
        }
    }
}