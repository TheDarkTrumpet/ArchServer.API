using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace External.tests.libTogglTests
{
    [TestClass]
    public class TimeEntries
    {
        private TimeEntriesMock _timeEntriesMock { get; set; }
        private Mock<RestRequest> MockRestRequest { get; set; }
        private Mock<RestClient> MockRestClient { get; set; }
        private Dictionary<string, List<Dictionary<string, string>>> InputObject;

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

            MockRestRequest = new Mock<RestRequest>();
            MockRestRequest = new Mock<RestRequest>();
            
            InputObject = new Dictionary<string, List<Dictionary<string, string>>>()
            {
                {
                    "items", new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>()
                        {
                            {"id", "123432"},
                            {"isBillable", "True"},
                            {"client", "A new client"},
                            {"user", "A user"},
                            {"project", "A new project"},
                            {"description", "A description of the task"},
                            {"billable", "5.23"},
                            {"start", "2020/01/01 15:00"},
                            {"end", "2020/01/01 17:00"}
                        }
                    }
                }
            };
        }

        private class TimeEntriesMock : libToggl.api.TimeEntries
        {
            public TimeEntriesMock(string apiKey) : base(apiKey) { }
            
            public TimeEntriesMock(string apiKey, string userAgent) : base(apiKey, userAgent) { }

            public string GetUserAgent => UserAgent;
        }
    }
}