using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            
        }

        private class MockWorkSpace : libToggl.api.Workspaces
        {
            public MockWorkSpace(string apiKey) : base(apiKey) { }
        }
    }
}