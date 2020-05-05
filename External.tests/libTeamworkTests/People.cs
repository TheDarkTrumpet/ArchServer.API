using libTeamwork.api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace External.tests.libTeamworkTests
{
    [TestClass]
    public class People
    {
        private libTeamwork.api.People _people { get; set; }
        public Mock<RestRequest> MockRestRequest { get; set; }
        
        public Mock<RestClient> MockRestClient { get; set; }
        
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
            
        }

        [TestMethod]
        public void GetPeople_WithAvailableElements_ShouldReturnListOfPeople()
        {
            
        }
        
        [TestInitialize]
        public void Initialize()
        {
            _people = new libTeamwork.api.People("AnAPIKey", "/peoplemock.json");
            MockRestRequest = new Mock<RestRequest>();
            MockRestClient = new Mock<RestClient>();
            MockRestClient.Setup(x => x.Execute(MockRestRequest.Object));
        }

        private class PeopleMock
        {
            
        }
    }
}