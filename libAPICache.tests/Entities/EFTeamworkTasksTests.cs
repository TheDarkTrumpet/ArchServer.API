using System.Collections.Generic;
using libTeamwork.api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libAPICache.tests.Entities
{
    public class EFTeamworkTasksTests : Base<Models.Teamwork.Task, ITasks>
    {

        [TestInitialize]
        public void Initialize()
        {
            Setup();
            _context.Setup(x => x.TeamworkTasks).Returns(_mockDbSet.Object);

            _iAPIMethod.Setup(x => x.GetTasks()).Returns(new List<libTeamwork.models.Task>());
            
        }
    }
}