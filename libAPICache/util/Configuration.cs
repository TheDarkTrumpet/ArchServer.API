using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace libAPICache.util
{
    public static class Configuration
    {
        public static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder().SetBasePath(@Directory.GetCurrentDirectory());
            return configurationBuilder
#if DEBUG
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/appsettings.Development.json", optional: true, reloadOnChange: true)
#else
                .AddJsonFile(@Directory.GetCurrentDirectory() + "{project path}/appsettings.json", optional: true, reloadOnChange: true)
#endif
                .Build();
        }
    }
}