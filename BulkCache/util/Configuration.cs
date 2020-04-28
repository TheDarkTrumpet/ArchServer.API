using Microsoft.Extensions.Configuration;

namespace BulkCache.Util
{
    public class Configuration
    {
        private IConfiguration configuration = libAPICache.util.Configuration.GetConfiguration();
        
    }
}