using System;
using libAPICache.Entities;
using libAPICache.Models;
using libAPICache.Models.Kimai;
using libKimai.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libAPICache.tests.Entities
{
    
    [TestClass]
    public class EFBaseTests
    {
        private Mock<EFDbContext> _context;
        private BaseMock _baseMock;
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
        
        
        [TestInitialize]
        public void Intialize()
        {
            _context = new Mock<EFDbContext>();
            _baseMock = new BaseMock(_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
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
            public EFDbContext InsertedContext { get; set; }

            public BaseMock(EFDbContext context) : base(context)
            {
                InsertedContext = context;
            }

            public void UpdateEnumerables(TimeEntry timeEntry, Activity activity)
            {
                EnumerablesTimesCalled += 1;
            }
        }
    }
}