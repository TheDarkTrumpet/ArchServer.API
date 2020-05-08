using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace External.tests.libTogglTests
{
    [TestClass]
    public class Workspaces
    {
        private MockWorkSpace _mockWorkspace;
        private Mock<RestRequest> MockRestRequest { get; set; }
        private Mock<RestClient> MockRestClient { get; set; }
        private List<Dictionary<string, string>> InputObject;

        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            Assert.IsNotNull(_mockWorkspace.ApiKey);
            Assert.IsNotNull(_mockWorkspace.RestClient);
            Assert.IsNotNull(_mockWorkspace.RestRequest);
            Assert.IsNotNull(_mockWorkspace.BaseURL);
            Assert.IsNotNull(_mockWorkspace.CookieContainer);
        }

        [TestMethod]
        public void GetRawWorkspaces_ShouldMakeCallAndReturnJArrayResults()
        {
            JArray results = _mockWorkspace.GetRawWorkspaces();
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void GetWorkspaces_ShouldMakeCallAndReturnListOfObjects()
        {
            
        }

        [TestMethod]
        [DataRow("Test Workspace 1", "12345")]
        [DataRow("Non-existent Workspace", null)]
        public void GetWorkspaceByName_WithInputs_ShouldReturn(string name, string expected)
        {
            
        }
        

        [TestInitialize]
        public void Initialize()
        {
            _mockWorkspace = new MockWorkSpace("An API Key");
            MockRestRequest = new Mock<RestRequest>();
            MockRestClient = new Mock<RestClient>();
            InputObject = new List<Dictionary<string, string>>()
            {
                {
                    new Dictionary<string, string>()
                    {
                        {"id", "12345"},
                        {"name", "Test Workspace 1"}
                    }
                },
                {
                    new Dictionary<string, string>()
                    {
                        {"id", "90000"},
                        {"name", "Test Workspace 2"}
                    }
                }
            };
            IRestResponse response = new RestResponse();
            response.Content = JsonConvert.SerializeObject(InputObject);

            MockRestClient.Setup(x => x.Execute(MockRestRequest.Object)).Returns(response);
            _mockWorkspace.SetupMocks(MockRestClient, MockRestRequest);
        }

        private class MockWorkSpace : libToggl.api.Workspaces
        {
            public MockWorkSpace(string apiKey) : base(apiKey) { }

            public void SetupMocks(Mock<RestClient> client, Mock<RestRequest> request)
            {
                RestClient = client.Object;
                RestRequest = request.Object;
            }
        }
    }
}