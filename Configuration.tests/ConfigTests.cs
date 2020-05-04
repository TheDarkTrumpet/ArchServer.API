using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configuration.tests
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void Config_WithFileNameAndDirectory_ShouldGetAndLoadConfiguration()
        {
            Config config = new Config();
            Console.WriteLine("Loaded file name from " + config.FullLoadedFileName);
            Assert.AreEqual("", config.FullLoadedFileName);
        }

        [TestMethod]
        [DataRow("appSettings.Example.json", "")]
        public void GetConfigurationFile_WithProperties_ShouldSetDefaults(string fileName, string directory)
        {
            
        }
        
        [TestInitialize]
        public void Initialize()
        {
            
        }
    }
}