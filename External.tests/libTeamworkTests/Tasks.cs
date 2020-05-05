using System.Collections.Generic;
using System.Linq;
using libTeamwork.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        [TestMethod]
        public void Constructor_WithAPIAndBase_ShouldSetProperties()
        {
            Assert.IsNotNull(_tasksMock.ApiKey);
            Assert.IsNotNull(_tasksMock.RestClient);
            Assert.IsNotNull(_tasksMock.CookieContainer);
            Assert.IsNotNull(_tasksMock.RestRequest);
            Assert.IsNotNull(_tasksMock.RestClient);
            Assert.IsNotNull(_tasksMock.BaseURL);
            Assert.IsNotNull(_tasksMock.EndPointURI);
        }

        [TestMethod]
        public void GetRawTasks_ShouldReturnJArrayObject()
        {
            JArray response = _tasksMock.GetRawTasks();
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
        }

        [TestMethod]
        public void GetTasks_WithAvailableElements_ShouldReturnListOfTasks()
        {
            List<Task> tasks = _tasksMock.GetTasks();
            
            Assert.IsNotNull(tasks);
            Assert.AreEqual(1, tasks.Count);

            Task result = tasks.First();
            Dictionary<string, string> resultReference = InputObject["todo-items"][0];
            
            Assert.AreEqual(resultReference["id"], result.Id.ToString());
            Assert.AreEqual(resultReference["project-name"], result.ProjectName);
            Assert.AreEqual(resultReference["company-name"], result.CompanyName);
            Assert.AreEqual(resultReference["content"], result.Title);
            Assert.AreEqual(resultReference["description"], result.Description);
            Assert.AreEqual(resultReference["priority"], result.Priority);
            Assert.AreEqual(resultReference["responsible-party-summary"], result.AssignedTo);
            Assert.AreEqual(resultReference["completed"], result.Completed.ToString());
            Assert.AreEqual(resultReference["created-on"], result.CreatedOn.Value.ToString("yyyy/MM/dd"));
            Assert.AreEqual(resultReference["due-date"], result.DueDate.Value.ToString("yyyyMMdd"));
        }
        
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
                            {"completed", "True"},
                            {"created-on", "2020/01/01"},
                            {"due-date", "20200201"}
                        
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