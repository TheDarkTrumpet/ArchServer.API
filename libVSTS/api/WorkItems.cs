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
                    Id = (long) rwi["id"],
                    //url = "", // Generate this...
                    Type = rwi["fields"]["System.WorkItemType"]?.ToString(),
                    State = rwi["fields"]["System.State"]?.ToString(),
                    Description = _sanitizeHTML(rwi["fields"]["System.Description"]?.ToString()),
                    AssignedTo = rwi["fields"]["System.AssignedTo"]?["displayName"]?.ToString(),
                    CreatedBy = rwi["fields"]["System.CreatedBy"]["displayName"]?.ToString()
                };

                DateTime parsedDate;
                if (DateTime.TryParse(rwi["fields"]["System.CreatedDate"]?.ToString(), out parsedDate))
                {
                    workItem.CreatedDate = parsedDate;
                }
                
                if (DateTime.TryParse(rwi["fields"]["System.ChangedDate"]?.ToString(), out parsedDate))
                {
                    workItem.ChangedDate = parsedDate;
                }
                
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
                    Id = (long) jcomment["id"],
                    CreatedBy = jcomment["createdBy"]?["displayName"]?.ToString(),
                    Comment = _sanitizeHTML(jcomment["text"]?.ToString())
                };

                DateTime parsedDate;
                if (DateTime.TryParse(jcomment["createdDate"]?.ToString(), out parsedDate))
                {
                    comment.CreatedDate = parsedDate;
                }
                
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

            string query = BuildQuery();
            var body = new Dictionary<string, string>() {
                {"query", query}
            };

            request.AddJsonBody(body);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = RestClient.Execute(request);
            var content = response.Content;

            // Convert the list of tasks received into a work item list.
            JObject contentJSON = JObject.Parse(content);
            return (JArray) contentJSON["workItems"];
        }

        public string BuildQuery()
        {
            string query = "Select [System.Id], [System.Title], [System.State] From WorkItems ";

            if (AssignedToInclude.Any() || TypesToInclude.Any() || StatesToExclude.Any())
            {
                query += " where ";
            }
            bool subFilterApplied = false;
            
            for (int x = 0; x < AssignedToInclude.Count; x++)
            {
                subFilterApplied = true;
                if (x == 0)
                {
                    query += "(";
                }
                query += $"[Assigned To] = '{AssignedToInclude[x]}'";

                if (x < AssignedToInclude.Count - 1)
                {
                    query += " or ";
                }
                else
                {
                    query += ") ";
                }
                
            }
            
            if (subFilterApplied && TypesToInclude.Any())
            {
                query += " and ";
            }
            
            for (int x = 0; x < TypesToInclude.Count; x++)
            {
                

                subFilterApplied = true;
                
                if (x == 0)
                {
                    query += "(";
                }
                query += $"[Work Item Type] = '{TypesToInclude[x]}'";

                if (x < TypesToInclude.Count - 1)
                {
                    query += " or ";
                }
                else
                {
                    query += ") ";
                }
            }
            
            if (subFilterApplied && StatesToExclude.Any())
            {
                query += " and ";
            }
            
            for (int x = 0; x < StatesToExclude.Count; x++)
            {
                if (x == 0)
                {
                    query += "(";
                }
                query += $"[State] <> '{StatesToExclude[x]}'";

                if (x < StatesToExclude.Count - 1)
                {
                    query += " and ";
                }
                else
                {
                    query += ") ";
                }
            }

            if (subFilterApplied && FromChanged != null)
            {
                query += " and ";
            }
            
            if (FromChanged != null)
            {
                query += $" [Changed Date] > '{FromChanged.Value.ToShortDateString()}'";
            }
            return query;
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