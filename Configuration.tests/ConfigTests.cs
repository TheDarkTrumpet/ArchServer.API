using System;
using System.Collections.Generic;
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
        public void GetKey_WithIdentifier_ShouldReturnIt()
        {
            Config config = new Config();

            string value = config.GetKey("APISources:Toggl:API_Key");
            
            Assert.IsNotNull(value);
            Assert.AreEqual("<API_KEY>", value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetKey_WithMissingIdentifier_ShouldThrowException()
        {
            Config config = new Config();

            string value = config.GetKey("DOES_NOT_EXIST!!!!");
            
            Assert.IsNotNull(value);
            Assert.AreEqual("<API_KEY>", value);
        }

        [TestMethod]
        public void GetInt_WithIdentifier_ShouldReturnIt()
        {
            Config config = new Config();

            int value = config.GetInt("APISources:Teamwork:FromDateDays");

            Assert.AreEqual(-5, value);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void GetInt_WithStringIdentifier_ShouldThrowException()
        {
            Config config = new Config();

            int value = config.GetInt("APISources:Teamwork:API_Key");

            Assert.AreEqual(-5, value);
        }

        [TestMethod]
        public void GetCollection_WithIdentifier_ShouldReturnStringList()
        {
            Config config = new Config();

            List<string> value = config.GetCollection("APISources:VSTS:AssignedToInclude");

            Assert.IsNotNull(value);
            Assert.AreEqual(2, value.Count);
        }
        
        [TestMethod]
        [DataRow("appSettings.Example.json", "/tmp/foo", "appSettings.Example.json", "/tmp/foo")]
        [DataRow("appSettings.Example.json", null, "appSettings.Example.json", "/bin/Debug")]
        [DataRow("appSettings.Example.json", "", "appSettings.Example.json", "/bin/Debug")]
        [DataRow("", "/tmp/foo", "appsettings.Development.json", "/tmp/foo")]
        [DataRow(null, null, "appsettings.Development.json", "/bin/Debug")]
        public void GetConfigurationFile_WithProperties_ShouldSetDefaults(string fileName, string directory,
            string expectedFileName, string expectedDir)
        {
            Config config = new ConfigMock();

            config.GetConfigurationFile(fileName, directory);

            Assert.IsTrue(config.FullLoadedFileName.Contains(expectedFileName));
            Assert.IsTrue(config.FullLoadedFileName.Contains(expectedDir));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LoadConfiguration_WithEmptyLoaded_ShouldThrowException()
        {
            Config config = new Config();
            config.FullLoadedFileName = null;
            
            config.LoadConfiguration();
        }

        [TestMethod]
        public void LoadConfiguration_WithLoadedFileName_ShouldCreateConfiguration()
        {
            Config config = new Config();
            config.LoadConfiguration();
            
            Assert.IsNotNull(config.LoadedConfiguration);
        }

        [TestInitialize]
        public void Initialize()
        {
            
        }

        private class ConfigMock : Config
        {
            public int GetConfigurationFileCalled { get; set; } = 0;
            public int LoadConfigurationCalled { get; set; } = 0;
            public override void GetConfigurationFile(string fileName = null, string directory = null)
            {
                base.GetConfigurationFile(fileName, directory);
                GetConfigurationFileCalled += 1;
            }

            public override void LoadConfiguration()
            {
                LoadConfigurationCalled += 1;
            }
        }
    }
}