using System;
using System.Collections.Generic;
using libAPICache.Entities;
using libToggl.api;
using libToggl.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libAPICache.tests.Entities
{
    [TestClass]
    public class EFTogglTimeEntriesTests : Base<Models.Toggl.TimeEntry, ITimeEntries>
    {
        [TestMethod]
        public void Constructor_ShouldSetPropertiesAndCallConfig()
        {
            EFTogglTimeEntries efTogglTimeEntries =
                new EFTogglTimeEntries(_context.Object, _config.Object, _iAPIMethod.Object);
            
            Assert.IsNotNull(efTogglTimeEntries.Entries);
            Assert.IsNotNull(efTogglTimeEntries.ApiKey);
            _config.Verify(x => x.GetKey(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [DataRow("Test workspace", null)]
        [DataRow("A different workspace", "2020/01/01")]
        public void CacheEntries_WithWorkspaceAndDatetime_ShouldCallGetAndSave(string workspaceName, string fromDateString)
        {
            EFTogglTimeEntries efTogglTimeEntries =
                new EFTogglTimeEntries(_context.Object, _config.Object, _iAPIMethod.Object);

            DateTime? fromDate = null;
            if (!string.IsNullOrEmpty(fromDateString))
            {
                fromDate = DateTime.Parse(fromDateString);
            }
            
            efTogglTimeEntries.CacheEntries(workspaceName, fromDate);
            
            _iAPIMethod.Verify(x => x.GetTimeEntries(workspaceName, fromDate, null), Times.Once);
            _context.Verify(x => x.SaveChanges(), Times.Once);
        }
        
        [TestInitialize]
        public void Initialize()
        {
            Setup();

            _iAPIMethod.Setup(x => x.GetTimeEntries(It.IsAny<string>(), It.IsAny<DateTime?>(), null))
                .Returns(new List<TimeEntry>());

            _config.Setup(x => x.GetKey("APISources:Toggl:API_Key")).Returns("Some key...");

            _context.Setup(x => x.TogglTimeEntries).Returns(_mockDbSet.Object);
        }
    }
}