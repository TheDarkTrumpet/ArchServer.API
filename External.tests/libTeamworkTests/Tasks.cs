using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace External.tests.libTeamworkTests
{
    [TestClass]
    public class Tasks
    {
        private TasksMock _tasksMock;
        private Mock<RestRequest> MockRestRequest { get; set; }
        private Mock<RestClient> MockRestClient { get; set; }
        private Dictionary<string, List<Dictionary<string, string>>> InputObject;        

        [TestInitialize]
        public void Initialize()
        {
            _tasksMock = new TasksMock("AnAPIKey", "/tasksmock.json");
            
        }

        private class TasksMock : libTeamwork.api.Tasks
        {
            public TasksMock(string apiKey, string baseUrl) : base(apiKey, baseUrl) { }

            public void SetupMocks(Mock<RestClient> client, Mock<RestRequest> request)
            {
                RestClient = client.Object;
                RestRequest = request.Object;
            }
        }
    }
}