using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace External.tests.libTogglTests
{
    [TestClass]
    public class TimeEntries
    {
        private TimeEntriesMock _timeEntriesMock { get; set; }

        [TestMethod]
        public void Constructor_WithAPIkey_ShouldSetProperties()
        {
            Assert.IsNotNull(_timeEntriesMock.ApiKey);
            Assert.IsNotNull(_timeEntriesMock.CookieContainer);
            Assert.IsNotNull(_timeEntriesMock.RestClient);
            Assert.IsNotNull(_timeEntriesMock.RestRequest);
            Assert.IsNotNull(_timeEntriesMock.BaseURL);
            Assert.IsNotNull(_timeEntriesMock);
        }

        [TestMethod]
        public void Constructor_WithAPIUserAgent_ShouldSetupAgent()
        {
            TimeEntriesMock timeEntriesMock = new TimeEntriesMock("Api Key Go", "America/Nowhere");
            
            Assert.IsNotNull(timeEntriesMock.GetUserAgent);
            Assert.AreEqual(timeEntriesMock.GetUserAgent, "America/Nowhere");
        }
        
        
        [TestInitialize]
        public void Initialize()
        {
            _timeEntriesMock = new TimeEntriesMock("An API Key");
        }

        private class TimeEntriesMock : libToggl.api.TimeEntries
        {
            public TimeEntriesMock(string apiKey) : base(apiKey) { }
            
            public TimeEntriesMock(string apiKey, string userAgent) : base(apiKey, userAgent) { }

            public string GetUserAgent => UserAgent;
        }
    }
}