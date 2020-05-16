using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using libToggl.api;
using libToggl.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace External.tests.libTogglTests
{
    [TestClass]
    public class TimeEntries
    {
        private TimeEntriesMock _timeEntriesMock { get; set; }
        private Mock<libToggl.api.Workspaces> _workspaces { get; set; }
        private Workspace _workspace { get; set; }
        private Mock<RestRequest> MockRestRequest { get; set; }
        private Mock<RestClient> MockRestClient { get; set; }
        private Dictionary<string, Object> InputObject;
        private int _pagesNeeded = 1;

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
            TimeEntriesMock timeEntriesMock = new TimeEntriesMock("Api Key Go", "user@email.com");
            
            Assert.IsNotNull(timeEntriesMock.GetUserAgent);
            Assert.AreEqual(timeEntriesMock.GetUserAgent, "user@email.com");
        }

        [TestMethod]
        public void GetRawTimeEntries_WithWorkspaceAndStartDateOnePage_ShouldReturnElements()
        {
            JArray results = _timeEntriesMock.GetRawTimeEntries(_workspace);
            
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void GetRawTimeEntries_WithWorkspaceAndStartDateTwoPage_ShouldReturnElements()
        {
            
        }

        [TestMethod]
        public void GetRawTimeEntries_WithWorkspaceAndNullStartDate_ShouldReturnElementsWithinMonth()
        {
            
        }

        [TestMethod]
        public void GetWorkspace_WithAvailableWorkspace_ShouldReturnIt()
        {
            Workspace workspace = _timeEntriesMock.CallGetWorkspace("Real Name");
            Assert.IsNotNull(workspace);
            Assert.AreEqual(5, workspace.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetWorkspace_WithUnavailableWorkspace_ShouldThrowException()
        {
            _timeEntriesMock.CallGetWorkspace("Does not exist!");
        }
        
        [TestInitialize]
        public void Initialize()
        {
            _workspaces = new Mock<libToggl.api.Workspaces>("An API Key");
            _workspace = new Workspace()
            {
                Id = 5,
                Name = "Real Name",
                Premium = true
            };
            _workspaces.Setup(x => x.GetWorkspaceByName("Real Name")).Returns(_workspace);
            _timeEntriesMock = new TimeEntriesMock("An API Key", _workspaces.Object);
            MockRestRequest = new Mock<RestRequest>();
            MockRestClient = new Mock<RestClient>();

            MockRestClient.Setup(x => x.Execute(MockRestRequest.Object))
                .Callback(() =>  this._getResponse()).Returns<IRestResponse>(x => x);
            _timeEntriesMock.SetupMocks(MockRestClient, MockRestRequest);
        }

        private IRestResponse _getResponse()
        {
            int requested_page = int.Parse(MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "page").Value.ToString());
            int num_per_page = 5;
            int total_count = _pagesNeeded * num_per_page;
            
            List<Dictionary<string, string>> entries = new List<Dictionary<string, string>>();
            entries.AddRange(Enumerable.Range(0, 5).Select(x => generateFixtureEntry()).ToList());
            
            InputObject = new Dictionary<string, Object>()
            {
                {"per_page", num_per_page.ToString()},
                {"total_count", total_count.ToString()},
                {"data", entries }
            };
            IRestResponse response = new RestResponse();
            response.Content = JsonConvert.SerializeObject(InputObject);
            return response;
        }

        private Dictionary<string, string> generateFixtureEntry()
        {
            Fixture autoFixture = new Fixture();
            Dictionary<string, string> result = new Dictionary<string, string>()
            {
                {"id", autoFixture.Create<int>().ToString()},
                {"isBillable", autoFixture.Create<bool>().ToString()},
                {"client", autoFixture.Create<string>()},
                {"user", autoFixture.Create<string>()},
                {"project", autoFixture.Create<string>()},
                {"description", autoFixture.Create<string>()},
                {"billable", autoFixture.Create<float>().ToString(CultureInfo.CurrentCulture)},
                {"start", autoFixture.Create<DateTime>().ToString("yyyy/MM/dd HH:mm")},
                {"end", autoFixture.Create<DateTime>().ToString("yyyy-MM-dd HH:mm")}
            };

            return result;
        }
        
        private class TimeEntriesMock : libToggl.api.TimeEntries
        {
            public TimeEntriesMock(string apiKey, libToggl.api.IWorkspaces workspaces = null) : base(apiKey, workspaces)
            {
            }

            public TimeEntriesMock(string apiKey, string userAgent, IWorkspaces workspaces = null) : base(apiKey, userAgent, workspaces)
            {
            }

            public string GetUserAgent => UserAgent;

            public Workspace CallGetWorkspace(string name)
            {
                return GetWorkspaceByName(name);
            }
            
            public void SetupMocks(Mock<RestClient> client, Mock<RestRequest> request)
            {
                RestClient = client.Object;
                RestRequest = request.Object;
            }
        }
    }
}