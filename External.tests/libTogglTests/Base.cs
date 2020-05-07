using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace External.tests.libTogglTests
{
    [TestClass]
    public class Base
    {

        [TestInitialize]
        public void Initialize()
        {
            
        }

        private class BaseMock : libToggl.api.Base
        {
            public BaseMock(string apiKey) : base(apiKey) { }
            
            
        }
    }
}