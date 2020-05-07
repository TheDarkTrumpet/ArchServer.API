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
        
        [TestInitialize]
        public void Initialize()
        {
            _base = new BaseMock("An API Key");
        }

        private class BaseMock : libToggl.api.Base
        {
            protected override string BaseUri { get; set; } = "/foobar";
            public BaseMock(string apiKey) : base(apiKey) { }
            
            
        }
    }
}