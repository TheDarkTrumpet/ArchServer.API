using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        private IRestResponse _restResponse { get; set; }
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
            DateTime startDate = DateTime.Now.AddMonths(-5);
            JArray results = _timeEntriesMock.GetRawTimeEntries(_workspace, startDate);
            string resultDate =
                startDate.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "Z";
            
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(_workspace.Id.ToString(),
                MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "workspace_id").Value);
            Assert.AreEqual(resultDate, MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "since").Value);
            Assert.AreEqual("1", MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "page").Value);
            Assert.AreEqual("none@nada.com", MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "user_agent").Value);
        }

        [TestMethod]
        public void GetRawTimeEntries_WithWorkspaceAndStartDateTwoPage_ShouldReturnElements()
        {
            _pagesNeeded = 5;
            JArray results = _timeEntriesMock.GetRawTimeEntries(_workspace);
            
            Assert.IsNotNull(results);
            Assert.AreEqual(25, results.Count);
            
        }

        [TestMethod]
        public void GetRawTimeEntries_WithWorkspaceAndNullStartDate_ShouldReturnElementsWithinMonth()
        {
            JArray results = _timeEntriesMock.GetRawTimeEntries(_workspace);
            string resultDate =
                DateTime.Now.AddMonths(-1).ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "Z";
            
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(_workspace.Id.ToString(),
                MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "workspace_id").Value);
            Assert.AreEqual(resultDate, MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "since").Value);
            Assert.AreEqual("1", MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "page").Value);
            Assert.AreEqual("none@nada.com", MockRestRequest.Object.Parameters.FirstOrDefault(x => x.Name == "user_agent").Value);
        }

        [TestMethod]
        public void GetRawTimeEntries_WithStringWorkspace_ShouldReturnList()
        {
            JArray results = _timeEntriesMock.GetRawTimeEntries(_workspace.Name);
            
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count);
        }

        [TestMethod]
        public void GetTimeEntries_Defaults_ShouldReturnElements()
        {
            IEnumerable<TimeEntry> results = _timeEntriesMock.GetTimeEntries(_workspace);
            
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count());
            
            //The below simply verifies each property was filled out, which acts as a trigger if we add more properties or change the load
            foreach(TimeEntry te in results.ToList())
            {
                foreach (PropertyInfo property in te.GetType().GetProperties())
                {
                    Assert.IsNotNull(property.GetValue(te));
                }
            }
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
                .Callback(() => _restResponse = this._getResponse()).Returns(() => _restResponse);
            _timeEntriesMock.SetupMocks(MockRestClient, MockRestRequest);
        }

        private IRestResponse _getResponse()
        {
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
                {"is_billable", autoFixture.Create<bool>().ToString()},
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