using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace External.tests.libTogglTests
{
    [TestClass]
    public class Base
    {
        private BaseMock _base { get; set; }

        [TestMethod]
        public void Constructor_ShouldSetPropertiesAndBuild()
        {
            Assert.IsNotNull(_base.ApiKey);
            Assert.IsNotNull(_base.CookieContainer);
            Assert.IsNotNull(_base.RestClient);
            Assert.IsNotNull(_base.RestRequest);
            Assert.IsNotNull(_base.BaseURL);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateClient_WithEmptyURL_ShouldThrowException()
        {
            _base.CallCreateClientWithoutURL();
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerateRestRequest_WithEmptyUri_ShouldThrowException()
        {
            _base.CallGenerateRestRequestWithoutURI();
        }
        
        [TestInitialize]
        public void Initialize()
        {
            _base = new BaseMock("An API Key");
        }

        private class BaseMock : libToggl.api.Base
        {
            protected override string BaseUri { get; set; } = "/foobar";
            public BaseMock(string apiKey) : base(apiKey) { }

            public void CallCreateClientWithoutURL()
            {
                BaseURL = null;
                CreateClient();
            }

            public void CallGenerateRestRequestWithoutURI()
            {
                BaseUri = null;
                GenerateRestRequest();
            }
        }
    }
}