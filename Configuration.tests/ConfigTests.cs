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
            ConfigMock config = new ConfigMock();
            Assert.IsTrue(config.FullLoadedFileName.Contains("appsettings.Development.json")); // Tests file
            Assert.IsTrue(config.FullLoadedFileName.Contains("/bin/Debug"));                   // Tests directory
            Assert.AreEqual(1, config.LoadConfigurationCalled);
            Assert.AreEqual(1, config.GetConfigurationFileCalled);
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

        private class ConfigMock : Config
        {
            public int GetConfigurationFileCalled { get; set; } = 0;
            public int LoadConfigurationCalled { get; set; } = 0;
            protected override void GetConfigurationFile(string fileName = null, string directory = null)
            {
                base.GetConfigurationFile(fileName, directory);
                GetConfigurationFileCalled += 1;
            }

            protected override void LoadConfiguration()
            {
                LoadConfigurationCalled += 1;
            }
        }
    }
}