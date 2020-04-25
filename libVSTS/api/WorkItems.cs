using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Html2Markdown;
using HtmlAgilityPack;
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
        public Boolean IncludeComments { get; set; }
        
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
                    Description = _sanitizeHTML(rwi["fields"]["System.Description"]?.ToString()),
                    AssignedTo = rwi["fields"]["System.AssignedTo"]?["displayName"]?.ToString(),
                    CreatedBy = rwi["fields"]["System.CreatedBy"]["displayName"].ToString(),
                    CreatedDate = (DateTime) rwi["fields"]["System.CreatedDate"],
                    ChangedDate = (DateTime) rwi["fields"]["System.ChangedDate"]
                };
                if (IncludeComments)
                {
                    workItem.Comments = _supplementComments(rwi);
                }
                workItems.Add(workItem);
            }

            return workItems;
        }

        private List<WorkItemComment> _supplementComments(JObject workItem)
        {
            string commentsLink = (string) workItem["_links"]["workItemComments"]["href"];
            RestRequest request = new RestRequest(commentsLink);
            IRestResponse response = RestClient.Execute(request);
            JObject content = JObject.Parse(response.Content);

            List<WorkItemComment> comments = new List<WorkItemComment>();
            foreach (JObject jcomment in content["comments"])
            {
                WorkItemComment comment = new WorkItemComment()
                {
                    id = (int) jcomment["id"],
                    CreatedBy = (string) jcomment["createdBy"]["displayName"],
                    CreatedDate = (DateTime) jcomment["createdDate"],
                    Comment = _sanitizeHTML(jcomment["text"]?.ToString())
                };
                comments.Add(comment);
            }

            return comments;
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

        private string _sanitizeHTML(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            input = input.Replace("<div>", "");
            input = input.Replace("</div>", "\n");
            input = Regex.Replace(input, "<[/]{0,1}span.*?>", "");

            try
            {
                Converter converter = new Converter();
                return converter.Convert(input);
            }
            catch (Exception)
            {
                return input;
            }
            
        }
    }
}