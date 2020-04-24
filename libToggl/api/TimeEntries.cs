using System;
using System.ComponentModel.Design;
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

        public JObject GetRawTimeEntries(string workspaceName, DateTime? startDate, DateTime? endDate)
        {
            Workspaces workspace_query = new Workspaces(ApiKey);
            Workspace workspace = workspace_query.GetWorkspaceIdByName(workspaceName);
            return GetRawTimeEntries(workspace, startDate, endDate);
        }

        public JObject GetRawTimeEntries(Workspace workspace, DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
            {
                startDate = DateTime.Now.AddMonths(-1);
            }
            
            RestRequest request = new RestRequest(_endpointUri, Method.GET);
            request.AddQueryParameter("workspace_id", workspace.Id.ToString());
            request.AddQueryParameter("since",
                startDate.Value.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "Z");
            request.AddQueryParameter("user_agent", _userAgent);
            
            IRestResponse response = RestClient.Execute(request);
            JObject results = JObject.Parse(response.Content);
            return results;
        }
    }
}