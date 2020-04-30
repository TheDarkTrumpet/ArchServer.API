using System;
using System.Collections.Generic;
using System.IO;
using libAPICache.Abstract;
using libAPICache.Entities;
using libAPICache.util;
using Microsoft.Extensions.Configuration;

namespace BulkCache.lib
{
    public class BulkCache
    {
        private readonly IConfiguration _configuration;
        private DateTime _timeLog;
        
        public BulkCache()
        {
            _configuration = Configuration.GetConfiguration();
        }

        private void WriteTimeLog(string operation, string state)
        {
            _timeLog = DateTime.Now;
            Console.WriteLine($"{operation} {state} at {_timeLog}");
        }
        public void CacheAll()
        {
            WriteTimeLog("CacheAll", "Starting");
            CacheKimai();
            CacheToggl();
            CacheTeamwork();
            CacheVSTS();
            WriteTimeLog("CacheAll", "Finishing");
        }

        private void CacheKimai()
        {
            WriteTimeLog("--CacheKimai", "Starting");
            IKimaiTimeEntries efKimai = new EFKimaiTimeEntries();
            efKimai.CacheEntries(GetFromDay("Kimai:FromDateDays"), GetFromConfig("Kimai:TimeZone"));
        }

        private void CacheToggl()
        {
            WriteTimeLog("--CacheToggl", "Starting");
            ITogglWorkspace efToggl = new EFTogglWorkspace();
            //TODO Add in cache by date here.
            efToggl.CacheEntries();
        }

        private void CacheTeamwork()
        {
            WriteTimeLog("--CacheTeamwork:People", "Starting");
            ITeamworkPeople efTeamworkPeople = new EFTeamworkPeople();
            efTeamworkPeople.CacheEntries();
            
            WriteTimeLog("--CacheTeamwork:Tasks", "Starting");
            ITeamworkTasks efTeamworkTasks = new EFTeamworkTasks();
            efTeamworkTasks.CacheEntries(GetFromDay("Teamwork:FromDateDays"),
                Boolean.Parse(GetFromConfig("Teamwork:IncludeCompleted")));
        }

        private void CacheVSTS()
        {
            WriteTimeLog("--CacheVSTS", "Starting");
            IVSTSWorkItems efVSTS = new EFVSTSWorkItems();
            
            List<string> assignedToInclude = GetFromCollection("VSTS:AssignedToInclude");
            List<string> statesToExclude = GetFromCollection("VSTS:StatesToExclude");
            List<string> typesToInclude = GetFromCollection("VSTS:TypesToInclude");
            DateTime fromDate = GetFromDay("VSTS:FromDateDays");
            bool includeComments = Boolean.Parse(GetFromConfig("VSTS:IncludeComments"));

            efVSTS.CacheEntries(includeComments, assignedToInclude, statesToExclude, typesToInclude, fromDate);
        }

        private List<string> GetFromCollection(string identifier)
        {
            List<string> returnValues = new List<string>();
            _configuration.GetSection($"APISources:{identifier}").Bind(returnValues);
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
            string fullLookup = $"APISources:{identifier}";
            string value = (string) _configuration[fullLookup];
            
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception(
                    $"{@Directory.GetCurrentDirectory()}/appsettings.json entry does not exist for {fullLookup}, please make sure it's defined!");
            }

            return value;
        }
    }
}