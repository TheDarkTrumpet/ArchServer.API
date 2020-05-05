using System.Collections.Generic;
using System.Linq;
using libTeamwork.api;
using libTeamwork.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace External.tests.libTeamworkTests
{
    [TestClass]
    public class People
    {
        private PeopleMock _people { get; set; }
        private Mock<RestRequest> MockRestRequest { get; set; }
        
        private Mock<RestClient> MockRestClient { get; set; }

        private Dictionary<string, List<Dictionary<string, string>>> InputObject;
        
        [TestMethod]
        public void Constructor_WithAPIAndBase_ShouldSetProperties()
        {
            Assert.IsNotNull(_people.ApiKey);
            Assert.IsNotNull(_people.RestClient);
            Assert.IsNotNull(_people.CookieContainer);
            Assert.IsNotNull(_people.RestRequest);
            Assert.IsNotNull(_people.RestClient);
            Assert.IsNotNull(_people.BaseURL);
            Assert.IsNotNull(_people.EndPointURI);
        }

        [TestMethod]
        public void GetRawPeople_ShouldReturnJArrayObject()
        {
            JArray response = _people.GetRawPeople();
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
        }

        [TestMethod]
        public void GetPeople_WithAvailableElements_ShouldReturnListOfPeople()
        {
            List<Person> people = _people.GetPeople();
            
            Assert.IsNotNull(people);
            Assert.AreEqual(1, people.Count);

            Person result = people.First();
            Dictionary<string, string> resultReference = InputObject["people"][0];
            Assert.AreEqual(resultReference["id"], result.Id.ToString());
            Assert.AreEqual(resultReference["user-name"], result.UserName);
            Assert.AreEqual(resultReference["full-name"], result.FullName);
            Assert.AreEqual(resultReference["email-address"], result.EmailAddress);
            Assert.AreEqual(resultReference["company-name"], result.CompanyName);
            Assert.AreEqual(resultReference["administrator"], result.Administrator.ToString());
            Assert.AreEqual(resultReference["last-login"], result.LastActive.Value.ToString("yyyy/MM/dd"));
        }
        
        [TestInitialize]
        public void Initialize()
        {
            _people = new PeopleMock("AnAPIKey", "/peoplemock.json");
            MockRestRequest = new Mock<RestRequest>();
            MockRestClient = new Mock<RestClient>();

            InputObject =
                new Dictionary<string, List<Dictionary<string, string>>>()
                {
                    {
                        "people", new List<Dictionary<string, string>>()
                        {
                            new Dictionary<string, string>()
                            {
                                {"id", "12345"},
                                {"user-name", "user@name.com"},
                                {"full-name", "User Name"},
                                {"email-address", "user@email-address.com"},
                                {"company-name", "company-name"},
                                {"administrator", "True"},
                                {"last-login", "2020/01/01"}
                            }
                        }
                    }
                };
            IRestResponse response = new RestResponse();
            response.Content = JsonConvert.SerializeObject(InputObject);
            
            MockRestClient.Setup(x => x.Execute(MockRestRequest.Object)).Returns(response);

            _people.SetupMocks(MockRestClient, MockRestRequest);
        }

        private class PeopleMock : libTeamwork.api.People
        {
            public PeopleMock(string apiKey, string baseUrl) : base(apiKey, baseUrl) { }

            public void SetupMocks(Mock<RestClient> client, Mock<RestRequest> request)
            {
                RestClient = client.Object;
                RestRequest = request.Object;
            }
        }
    }
}