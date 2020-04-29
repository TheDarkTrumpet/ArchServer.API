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
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateEnumerables_NoOverride_ShouldThrowException()
        {
            BaseMockWithoutEnumerables input = new BaseMockWithoutEnumerables();
            input.UpdateEnumerables(new Activity(), new TimeEntry());
        }

        [TestInitialize]
        private void Intialize()
        {
            
        }

        [TestCleanup]
        private void Cleanup()
        {
            
        }
        private class BaseMockWithoutEnumerables : EFBase<Models.Kimai.TimeEntry, libKimai.models.Activity>
        {
            //Empty!
        }
    }
}