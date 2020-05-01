using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace Configuration
{
    public class Config : IConfig
    {
        protected string ConfigurationFile { get; set; }
        protected IConfiguration LoadedConfiguration { get; set; }
        
        public Config(string fileName = null, string directory = null)
        {
            ConfigurationFile = GetConfigurationFile(fileName, directory);
            LoadConfiguration();
        }

        public virtual string GetKey(string identifier)
        {
            string value = (string) LoadedConfiguration[identifier];

            if (String.IsNullOrEmpty(value))
            {
                throw new Exception($"Attempted to read, {identifier} from {ConfigurationFile}, which was not found");
            }

            return value;
        }

        public virtual int GetInt(string identifier)
        {
            string value = GetKey(identifier);
            int intvalue = int.Parse(value);
            return intvalue;
        }

        public virtual List<string> GetCollection(string identifier)
        {
            List<string> returnValues = new List<string>();
            LoadedConfiguration.GetSection($"{identifier}").Bind(returnValues);
            return returnValues;
        }
        
        protected virtual string GetConfigurationFile(string fileName = null, string directory = null)
        {
            if (String.IsNullOrEmpty(directory))
            {
                directory = @Directory.GetCurrentDirectory();
            }
            
            if (String.IsNullOrEmpty(fileName))
            {
#if DEBUG
                return $"{directory}/appsettings.Development.json";
#else
                return $"{directory}/appsettings.json";
#endif
            }

            return $"{directory}/{fileName}";
        }
        
        protected virtual void LoadConfiguration()
        {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder().SetBasePath(@Directory.GetCurrentDirectory());
            LoadedConfiguration = configurationBuilder
                .AddJsonFile(ConfigurationFile, optional: true, reloadOnChange: true).Build();
        }
    }
}