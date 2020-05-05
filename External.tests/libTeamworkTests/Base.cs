using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using libTeamwork.api;

namespace External.tests.libTeamworkTests
{
    [TestClass]
    public class Base
    {
        private BaseMock _baseMock;
        
        [TestMethod]
        public void Constructor_WithParameters_ShouldCreateVariables()
        {
            Assert.IsNotNull(_baseMock.ApiKey);
            Assert.IsNotNull(_baseMock.BaseURL);
            Assert.IsNotNull(_baseMock.CookieContainer);
        }

        [TestMethod]
        public void CreateClient_ShouldCreateClientAndSetProperties()
        {
            _baseMock.CallCreateClient();
            
            Assert.IsNotNull(_baseMock.RestClient);
            Assert.IsNotNull(_baseMock.RestClient.Authenticator);
            Assert.IsNotNull(_baseMock.RestClient.CookieContainer);
        }

        [TestMethod]
        public void GenerateRestRequest_WithEndpointURIDefined_ShouldCreateRequest()
        {
            _baseMock.SetEndpointURIAndCallRestRequest();
            Assert.IsNotNull(_baseMock.RestRequest);
            Assert.AreEqual("/foo/bar", _baseMock.RestRequest.Resource);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerateRestRequest_WithEndpointURIUndefined_ShouldThrowException()
        {
            _baseMock.SetEndpointURINullAndCallRestRequest();
        }
        
        [TestInitialize]
        public void Initialize()
        {
            _baseMock  = new BaseMock("A Key", "http://google.com");
        }

        private class BaseMock : libTeamwork.api.Base
        {
            public BaseMock(string apiKey, string baseUrl) : base(apiKey, baseUrl)
            {
            }

            public void CallCreateClient()
            {
                base.CreateClient();
            }

            public void SetEndpointURINullAndCallRestRequest()
            {
                EndPointURI = null;
                GenerateRestRequest();
            }

            public void SetEndpointURIAndCallRestRequest()
            {
                EndPointURI = "/foo/bar";
                GenerateRestRequest();
            }
        }
    }
}