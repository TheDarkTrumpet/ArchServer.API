using System;
using System.Collections.Generic;
using Configuration;
using libAPICache.Abstract;
using libAPICache.Entities;

namespace BulkCache.lib
{
    public class BulkCache
    {
        private readonly IConfig _configuration;
        private DateTime _timeLog;
        
        public BulkCache()
        {
            _configuration = new Config();
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
            efKimai.CacheEntries(GetFromDay("Kimai:FromDateDays"), _configuration.GetKey("APISources:Kimai:TimeZone"));
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
                Boolean.Parse(_configuration.GetKey("APISources:Teamwork:IncludeCompleted")));
        }

        private void CacheVSTS()
        {
            WriteTimeLog("--CacheVSTS", "Starting");
            IVSTSWorkItems efVSTS = new EFVSTSWorkItems();
            
            List<string> assignedToInclude = _configuration.GetCollection("APISources:VSTS:AssignedToInclude");
            List<string> statesToExclude = _configuration.GetCollection("APISources:VSTS:StatesToExclude");
            List<string> typesToInclude = _configuration.GetCollection("APISources:VSTS:TypesToInclude");
            DateTime fromDate = GetFromDay("VSTS:FromDateDays");
            bool includeComments = Boolean.Parse(_configuration.GetKey("APISources:VSTS:IncludeComments"));

            efVSTS.CacheEntries(includeComments, assignedToInclude, statesToExclude, typesToInclude, fromDate);
        }

        private DateTime GetFromDay(string identifier)
        {
            int fromDay = _configuration.GetInt($"APISources:{identifier}");
            return DateTime.Now.AddDays(fromDay);
        }
    }
}