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
        
        [TestInitialize]
        public void Initialize()
        {
            _mock = new BaseMock();
        }
        
        private class BaseMock : libVSTS.api.Base
        {
            public BaseMock() : this("anAPIKey", "anOrganization") { }
            public BaseMock(string apiKey, string organization) : base(apiKey,organization) { }
        }
    }
}