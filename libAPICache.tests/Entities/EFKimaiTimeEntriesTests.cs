using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Configuration;
using libAPICache.Entities;
using libAPICache.Models.Kimai;
using libAPICache.tests.Helpers;
using libKimai.models;
using libKimai.query;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libAPICache.tests.Entities
{
    [TestClass]
    public class EFKimaiTimeEntriesTests
    {
        private Mock<EFDbContext> _context;
        private Mock<DbSet<Models.Kimai.TimeEntry>> _mockDbSet;
        private Mock<IConfig> _config;
        private Mock<IActivities> _iActivities;
        private DateTime? _date;
        
        [TestMethod]
        [DataRow("2020/01/01")]
        [DataRow(null)]
        public void CacheEntries_WithDateTime_ShouldCallGetActivitiesAndSave(string inputTime)
        {
            EFKimaiTimeEntries efKimaiTimeEntries =
                new EFKimaiTimeEntries(_context.Object, _config.Object, _iActivities.Object);

            DateTime? inputDate = null;
            if (!string.IsNullOrEmpty(inputTime))
            {
                inputDate = DateTime.Parse(inputTime);
            }
            
            efKimaiTimeEntries.CacheEntries(inputDate);
            
            Assert.AreEqual(inputDate, _date);
            _iActivities.Verify(x => x.GetActivities(false), Times.Once);
        }

        [TestInitialize]
        public void Initialize()
        {
            Fixture autoFixture = new Fixture();
            IQueryable<TimeEntry> timeEntries = autoFixture.CreateMany<TimeEntry>().AsQueryable();
            
            _mockDbSet = GenerateDBSetHelper<TimeEntry>.GenerateDbSet(timeEntries);
            
            _context = new Mock<EFDbContext>();
            _context.Setup(x => x.KimaiTimeEntries).Returns(_mockDbSet.Object);
            
            _iActivities = new Mock<IActivities>();
            _iActivities.Setup(x => x.GetActivities(false)).Returns(new List<Activity>());
            _iActivities.SetupSet(x => x.FromDate = It.IsAny<DateTime?>()).Callback<DateTime?>(v => _date = v);
            _config = new Mock<IConfig>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            
        }
    }
}