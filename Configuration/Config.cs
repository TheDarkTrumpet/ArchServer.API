using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace Configuration
{
    public class Config : IConfig
    {
        public string FullLoadedFileName { get; set; }
        public IConfiguration LoadedConfiguration { get; private set; }
        
        public Config(string fileName = null, string directory = null)
        {
            SetConfigurationFile(fileName, directory);
            LoadConfiguration();
        }

        public virtual string GetKey(string identifier)
        {
            string value = (string) LoadedConfiguration[identifier];

            if (String.IsNullOrEmpty(value))
            {
                throw new Exception($"Attempted to read, {identifier} from {FullLoadedFileName}, which was not found");
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
        
        public virtual void SetConfigurationFile(string fileName = null, string directory = null)
        {
            if (String.IsNullOrEmpty(directory))
            {
                directory = @Directory.GetCurrentDirectory();
            }
            
            if (String.IsNullOrEmpty(fileName))
            {
#if DEBUG
                fileName = "appsettings.Development.json";
#else
                fileName = "appsettings.json";
#endif
            }

            FullLoadedFileName = $"{directory}/{fileName}";
        }
        
        public virtual void LoadConfiguration()
        {
            if (String.IsNullOrEmpty(FullLoadedFileName))
            {
                throw new Exception("The file hasn't been defined, yet, so we can't load an empty definition");
            }
            
            IConfigurationBuilder configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder().SetBasePath(@Directory.GetCurrentDirectory());
            LoadedConfiguration = configurationBuilder
                .AddJsonFile(FullLoadedFileName, optional: true, reloadOnChange: true).Build();
        }
    }
}