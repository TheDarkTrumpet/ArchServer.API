using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace External.tests.libVSTSTests
{
    public class Base
    {


        [TestInitialize]
        public void Initialize()
        {
            
        }
        
        private class BaseMock : libVSTS.api.Base
        {
            public BaseMock(string apiKey, string organization) : base(apiKey,organization) { }
        }
    }
}