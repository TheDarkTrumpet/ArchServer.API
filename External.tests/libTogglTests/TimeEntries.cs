using System;
using System.Collections.Generic;
using libToggl.api;
using libToggl.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace External.tests.libTogglTests
{
    [TestClass]
    public class TimeEntries
    {
        private TimeEntriesMock _timeEntriesMock { get; set; }
        private Mock<Workspaces> _workspace { get; set; }
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
            TimeEntriesMock timeEntriesMock = new TimeEntriesMock("Api Key Go", "user@email.com");
            
            Assert.IsNotNull(timeEntriesMock.GetUserAgent);
            Assert.AreEqual(timeEntriesMock.GetUserAgent, "user@email.com");
        }

        [TestMethod]
        public void GetRawTimeEntries_WithWorkspaceAndStartDateOnePage_ShouldReturnElements()
        {
            
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
            _workspace = new Mock<Workspaces>("An API Key");
            _workspace.Setup(x => x.GetWorkspaceByName("Real Name")).Returns(new Workspace()
            {
                Id = 5,
                Name = "Real Name",
                Premium = true
            });
            _timeEntriesMock = new TimeEntriesMock("An API Key", _workspace.Object);
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
            public TimeEntriesMock(string apiKey, IWorkspaces workspaces = null) : base(apiKey, workspaces)
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
        }
    }
}