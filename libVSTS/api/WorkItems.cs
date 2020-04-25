using System;
using System.Collections.Generic;
using libVSTS.models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace libVSTS.api
{
    public class WorkItems : Base
    {
        private readonly string _endpointUri;
        public List<string> StatesToExclude { get; set; }
        public List<string> AssignedToInclude { get; set; }
        public List<string> TypesToInclude { get; set; }
        public DateTime? FromChanged { get; set; }
        
        public WorkItems(string apiKey, string organization, string project) : base(apiKey, organization)
        {
            StatesToExclude = new List<string>();
            AssignedToInclude = new List<string>();
            TypesToInclude = new List<string>();
            _endpointUri = $"{project}/_apis/wit/wiql";
            CreateClient();
        }

        public JArray GetRawWorkItems()
        {
            JArray workItemLinks = _getWorkItemLinks();
            
            JArray workItems = new JArray();
            foreach (var wil in workItemLinks)
            {
                RestRequest request = new RestRequest((string)wil["url"]);
                IRestResponse response = RestClient.Execute(request);
                JObject content = JObject.Parse(response.Content);
                workItems.Add(content);
            }
            return workItems;
        }

        public List<WorkItem> GetWorkItems()
        {
            JArray rawWorkItems = GetRawWorkItems();
            List<WorkItem> workItems = new List<WorkItem>();

            foreach (JObject rwi in rawWorkItems)
            {
                WorkItem workItem = new WorkItem()
                {
                    id = (int) rwi["id"],
                    //url = "", // Generate this...
                    Type = rwi["fields"]["System.WorkItemType"].ToString(),
                    State = rwi["fields"]["System.State"].ToString(),
                    Description = rwi["fields"]["System.Description"]?.ToString(),
                    AssignedTo = rwi["fields"]["System.AssignedTo"]?["displayName"]?.ToString(),
                    CreatedBy = rwi["fields"]["System.CreatedBy"]["displayName"].ToString(),
                    CreatedDate = (DateTime) rwi["fields"]["System.CreatedDate"],
                    ChangedDate = (DateTime) rwi["fields"]["System.ChangedDate"]
                };
                workItems.Add(workItem);
            }

            return workItems;
        }

        private JArray _getWorkItemLinks()
        {
            RestRequest request = new RestRequest(_endpointUri, Method.POST);
            request.AddQueryParameter("api-version", "5.1");
            request.AddQueryParameter("$depth", "2");
            request.AddQueryParameter("$expand", "all");
            request.AddHeader("Content-Type", "application/json");
            
            var body = new Dictionary<string, string>() {
                {"query", "Select [System.Id], [System.Title], [System.State] From WorkItems Where ([Assigned To] = 'dthole@gmail.com' or [Assigned To] = 'dthole@uiowa.edu')" +
                          "and [Work Item Type] = 'Product Backlog Item' and [State] <> 'Done' and [State] <> 'Closed' and [State] <> 'Removed'"}
            };

            request.AddJsonBody(body);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = RestClient.Execute(request);
            var content = response.Content;

            // Convert the list of tasks received into a work item list.
            JObject contentJSON = JObject.Parse(content);
            return (JArray) contentJSON["workItems"];
        }
    }
}