using AutoFixture;
using libAPICache.Models.Kimai;
using libAPICache.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libAPICache.tests.Models.Kimai
{
    [TestClass]
    public class TimeEntryTests
    {
        [TestMethod]
        public void TestProperties()
        {
            var fixture = new Fixture();
            var expected = fixture.Build<TimeEntry>()
                .OmitAutoProperties()
                .Create();
            TimeEntry result = new TimeEntry().Copy(expected);
            
            Assert.AreSame(expected, result);
        }
    }
}