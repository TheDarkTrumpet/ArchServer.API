using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using libToggl.models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace libToggl.api
{
    public class TimeEntries : Base
    {
        public TimeEntries(string apiKey) : base(apiKey)
        {
            BaseURL = "https://www.toggl.com/reports/api/v2";
            _userAgent = "none@nada.com";
            CreateClient();
        }

        public TimeEntries(string apiKey, string userAgent) : this(apiKey)
        {
            _userAgent = userAgent;
        }
        
        private readonly string _endpointUri = "/details";
        private string _userAgent { get; set; }

        public JArray GetRawTimeEntries(string workspaceName, DateTime? startDate = null, DateTime? endDate = null)
        {
            return GetRawTimeEntries(_getWorkspace(workspaceName), startDate, endDate);
        }

        public JArray GetRawTimeEntries(Workspace workspace, DateTime? startDate = null, DateTime? endDate = null,
            int page = 1)
        {
            if (startDate == null)
            {
                startDate = DateTime.Now.AddMonths(-1);
            }
            
            RestRequest request = new RestRequest(_endpointUri, Method.GET);
            request.AddQueryParameter("workspace_id", workspace.Id.ToString());
            request.AddQueryParameter("since",
                startDate.Value.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "Z");
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("user_agent", _userAgent);
            
            IRestResponse response = RestClient.Execute(request);
            JObject baseObject = JObject.Parse(response.Content);
            JArray results = (JArray) baseObject["data"];
            
            //Calculate the remaining, by what page we're on, and recursively call if needed
            int total_count = (int) baseObject["total_count"];
            int per_page = (int) baseObject["per_page"];

            if (total_count > (per_page * page))
            {
                results.Merge(GetRawTimeEntries(workspace, startDate, endDate, page + 1));
            }
            
            return results;
        }

        public IEnumerable<TimeEntry> GetTimeEntries(Workspace workspace, DateTime? startDate = null, DateTime? endDate = null)
        {
            JArray rawEntries = GetRawTimeEntries(workspace, startDate, endDate);
            
            List<TimeEntry> timeEntries = new List<TimeEntry>();
            foreach(JObject rt in rawEntries)
            {
                TimeEntry timeEntry = new TimeEntry()
                {
                    Id = (long) rt["id"],
                    IsBillable = (bool) rt["is_billable"],
                    Client = (string) rt["client"],
                    User = (string) rt["user"],
                    Project = (string) rt["project"],
                    Description = rt["description"]?.ToString()
                };

                if (timeEntry.IsBillable)
                {
                    timeEntry.Billable = (double) rt["billable"];
                }

                string datetime = rt["start"].ToString();
                if (!String.IsNullOrEmpty(datetime))
                {
                    timeEntry.StartDate = DateTime.Parse(datetime);
                }
                
                datetime = rt["end"].ToString();
                if (!String.IsNullOrEmpty(datetime))
                {
                    timeEntry.EndDate = DateTime.Parse(datetime);
                }

                timeEntries.Add(timeEntry);
            }

            return timeEntries;
        }

        public IEnumerable<TimeEntry> GetTimeEntries(string name, DateTime? startDate = null, DateTime? endDate = null)
        {
            Workspace workspace = _getWorkspace(name);
            return GetTimeEntries(workspace, startDate, endDate);
        }

        private Workspace _getWorkspace(string name)
        {
            Workspaces workspace_query = new Workspaces(ApiKey);
            Workspace workspace = workspace_query.GetWorkspaceIdByName(name);
            return workspace;
        }
    }
}