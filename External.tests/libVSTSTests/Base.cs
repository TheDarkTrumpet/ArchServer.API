using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace External.tests.libVSTSTests
{
    [TestClass]
    public class Base
    {
        private BaseMock _mock { get; set; }

        [TestMethod]
        public void Constructor_WithArgs_ShouldSetParameters()
        {
            Assert.IsNotNull(_mock.ApiKey);
            Assert.AreEqual("anAPIKey", _mock.ApiKey);
            Assert.IsNotNull("anOrganization", _mock.Organization);
        }

        [TestMethod]
        public void CreateClient_NoArgs_ShouldCreateClient()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateClient_NoArgsNoBaseURL_ShouldThrowException()
        {
            
        }

        [TestMethod]
        public void GenerateRestRequest_NoArgsBaseUri_ShouldCreateRestRequest()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerateRestRequest_NoArgsNoUri_ShouldThrowException()
        {
            
        }
        
        [TestInitialize]
        public void Initialize()
        {
            _mock = new BaseMock();
        }
        
        private class BaseMock : libVSTS.api.Base
        {
            public BaseMock() : this("anAPIKey", "anOrganization", "aProject") { }
            public BaseMock(string apiKey, string organization, string project) : base(apiKey,organization, project) { }
        }
    }
}