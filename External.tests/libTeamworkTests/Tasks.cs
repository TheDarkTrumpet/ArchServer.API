using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
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
            MockRestRequest = new Mock<RestRequest>();
            MockRestClient = new Mock<RestClient>();

            InputObject = new Dictionary<string, List<Dictionary<string, string>>>()
            {
                {
                    "todo-items", new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>()
                        {
                            {"id", "12345"},
                            {"project-name", "A Project Name"},
                            {"company-name", "A Company Name"},
                            {"content", "Some content..."},
                            {"description", "A description of the task"},
                            {"priority", "High"},
                            {"responsible-party-summary", "user@email.com"},
                            {"completed", "true"},
                            {"created-on", "2020/01/01"},
                            {"due-date", "2020/02/01"}
                        
                        }
                    }
                }
            };
            
            IRestResponse response = new RestResponse();
            response.Content = JsonConvert.SerializeObject(InputObject);

            MockRestClient.Setup(x => x.Execute(MockRestRequest.Object)).Returns(response);
            _tasksMock.SetupMocks(MockRestClient, MockRestRequest);
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