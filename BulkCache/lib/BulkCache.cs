using System;
using System.Collections.Generic;
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
            efKimai.CacheEntries(GetFromDay("Kimai:FromDateDays"));
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

        private List<string> GetFromCollection(string identifier)
        {
            List<string> returnValues = new List<string>();

            return returnValues;
        }
        
        private DateTime GetFromDay(string identifier)
        {
            string value = GetFromConfig(identifier);
            int fromDay = int.Parse(value);

            return DateTime.Now.AddDays(fromDay);
        }
        
        private string GetFromConfig(string identifier)
        {
            string fullLookup = $"APISources:identifier";
            string value = (string) _configuration[fullLookup];
            
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception(
                    $"appsettings.json entry does not exist for {identifier}, please make sure it's defined!");
            }

            return value;
        }
    }
}