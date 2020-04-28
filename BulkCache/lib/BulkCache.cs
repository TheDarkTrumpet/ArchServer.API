using libAPICache.Abstract;
using libAPICache.Entities;
using libAPICache.util;
using Microsoft.Extensions.Configuration;

namespace BulkCache.lib
{
    public class BulkCache
    {
        private readonly IConfiguration _configuration;

        public BulkCache()
        {
            _configuration = Configuration.GetConfiguration();
        }

        public void CacheAll()
        {
            CacheKimai();
            CacheToggl();
            CacheTeamwork();
            CacheVSTS();
        }
        
        protected void CacheKimai()
        {
            IKimaiTimeEntries efKimai = new EFKimaiTimeEntries();
            efKimai.CacheEntries();
        }

        protected void CacheToggl()
        {
            
        }

        protected void CacheTeamwork()
        {
            
        }

        protected void CacheVSTS()
        {
            
        }
    }
}