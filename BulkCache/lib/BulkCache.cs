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
            //CacheToggl();
            //CacheTeamwork();
            CacheVSTS();
            WriteTimeLog("CacheAll", "Finishing");
        }

        private void CacheKimai()
        {
            WriteTimeLog("--CacheKimai", "Starting");
            IKimaiTimeEntries efKimai = new EFKimaiTimeEntries();
            efKimai.CacheEntries(GetFromDay("Kimai:FromDateDays"));
        }

        private void CacheToggl()
        {
            WriteTimeLog("--CacheToggl", "Starting");
            ITogglWorkspace efToggl = new EFTogglWorkspace();
            efToggl.CacheEntries();
            
            ITogglTimeEntries efTimeEntries = new EFTogglTimeEntries();
            string workspace = _configuration.GetKey("APISources:Toggl:Workspace");
            string fromDateDaysString = _configuration.GetKey("APISources:Toggl:FromDateDays");

            int? fromDateDays = null;
            if (!String.IsNullOrEmpty(fromDateDaysString))
            {
                try
                {
                    fromDateDays = int.Parse(fromDateDaysString);
                }
                catch (Exception e)
                {
                    throw new InvalidCastException(
                        "Unable to convert the integer (intended) value in the APISources:Toggl:FromDateDays setting in the appSettings");
                }
            }

            efTimeEntries.CacheEntries(workspace, fromDateDays);
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