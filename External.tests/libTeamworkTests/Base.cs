using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using libTeamwork.api;

namespace External.tests.libTeamworkTests
{
    [TestClass]
    public class Base
    {
        [TestMethod]
        public void Constructor_WithParameters_ShouldCreateVariables()
        {
            BaseMock baseMock  = new BaseMock("A Key", "http://google.com");
            
            Assert.IsNotNull(baseMock.ApiKey);
            Assert.IsNotNull(baseMock.BaseURL);
            Assert.IsNotNull(baseMock.CookieContainer);
        }

        private class BaseMock : libTeamwork.api.Base
        {

            public BaseMock(string apiKey, string baseUrl) : base(apiKey, baseUrl)
            {
            }
        }
    }
}