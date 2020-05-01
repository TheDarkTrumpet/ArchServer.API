using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace libAPICache.util
{
    public interface IConfiguration
    {
        
    }
    
    public class Configuration : IConfiguration
    {
        protected string configurationFile { get; set; }
        protected IConfigurationRoot loadedConfiguration { get; set; }
        
        public Configuration(string fileName = null, string directory = null)
        {
            configurationFile = GetConfigurationFile(fileName, directory);
        }

        public virtual string GetKey(string identifier)
        {
            string value = (string) loadedConfiguration[identifier];

            if (String.IsNullOrEmpty(value))
            {
                throw new Exception($"Attempted to read, {identifier} from {configurationFile}, which was not found");
            }

            return value;
        }

        public virtual int GetInt(string identifier)
        {
            int value = int.Parse(loadedConfiguration[identifier]);
            return value;
        }
        
        protected string GetConfigurationFile(string fileName = null, string directory = null)
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
        
        protected void LoadConfiguration()
        {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder().SetBasePath(@Directory.GetCurrentDirectory());
            
            loadedConfiguration = configurationBuilder
                .AddJsonFile(configurationFile, optional: true, reloadOnChange: true).Build();
        }
    }
}