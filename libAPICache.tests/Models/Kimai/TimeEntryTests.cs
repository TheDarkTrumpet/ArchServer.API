using AutoFixture;
using AutoFixture.Dsl;
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
            Fixture fixture = new Fixture();
            TimeEntry expected = fixture.Build<TimeEntry>()
                .OmitAutoProperties()
                .Create();

            TimeEntry result = expected;

            Assert.AreSame(expected, result);
        }
    }
}