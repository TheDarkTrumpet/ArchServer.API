using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
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