using System;
using System.Collections.Generic;
using libAPICache.Entities;
using libTeamwork.api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libAPICache.tests.Entities
{
    [TestClass]
    public class EFTeamworkTasksTests : Base<Models.Teamwork.Task, ITasks>
    {
        private DateTime? _updatedAfterDate;
        private bool _includeCompleted;

        [TestMethod]
        public void EFTeamworkTasks_WithProperties_ShouldSet()
        {
            EFTeamworkTasks efTeamworkTasks = new EFTeamworkTasks(_context.Object, _config.Object, _iAPIMethod.Object);
            
            Assert.IsNotNull(efTeamworkTasks.ApiKey);
            Assert.IsNotNull(efTeamworkTasks.BaseUrl);
            Assert.IsNotNull(efTeamworkTasks.Entries);
        }

        [TestMethod]
        [DataRow("2020/01/01", true)]
        [DataRow(null, true)]
        [DataRow("2020/01/01", false)]
        [DataRow(null, false)]
        public void CacheEntries_WithMultipleProperties_ShouldGetTasksAndSave(string fromDate, bool includeCompleted)
        {
            EFTeamworkTasks efTeamworkTasks = new EFTeamworkTasks(_context.Object, _config.Object, _iAPIMethod.Object);

            DateTime? inputDate = null;
            if (!String.IsNullOrEmpty(fromDate))
            {
                inputDate = DateTime.Parse(fromDate);
            }
            
            efTeamworkTasks.CacheEntries(inputDate, includeCompleted);
            
            _iAPIMethod.Verify(x => x.GetTasks(), Times.Once);

            if (inputDate == null)
            {
                Assert.IsNull(_updatedAfterDate);
            }
            else
            {
                Assert.AreEqual(inputDate, _updatedAfterDate);    
            }
            
            Assert.AreEqual(includeCompleted, _includeCompleted);
        }
        
        [TestInitialize]
        public void Initialize()
        {
            Setup();
            _context.Setup(x => x.TeamworkTasks).Returns(_mockDbSet.Object);

            _iAPIMethod.Setup(x => x.GetTasks()).Returns(new List<libTeamwork.models.Task>());
            _iAPIMethod.SetupSet(x => x.UpdatedAfterDate = It.IsAny<DateTime?>()).Callback<DateTime?>(v => _updatedAfterDate = v);
            _iAPIMethod.SetupSet(x => x.IncludeCompleted = It.IsAny<bool>()).Callback<bool>(v => _includeCompleted = v);
            
            _config.Setup(x => x.GetKey("APISources:Teamwork:API_Key")).Returns("An API Key");
            _config.Setup(x => x.GetKey("APISources:Teamwork:Base_URL")).Returns("A Base URL");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _includeCompleted = false;
            _updatedAfterDate = null;
        }
    }
}