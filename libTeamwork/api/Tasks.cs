using System;
using System.Collections.Generic;
using System.Globalization;
using libTeamwork.models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace libTeamwork.api
{
    public class Tasks : Base, ITasks
    {
        public bool IncludeCompleted { get; set; } = true;
        public DateTime? UpdatedAfterDate { get; set; }

        public Tasks(string apiKey, string baseUrl) : base(apiKey, baseUrl)
        {
            EndPointURI = "/tasks.json";
            CreateClient();
            GenerateRestRequest();
        }

        public JArray GetRawTasks()
        {
            if (UpdatedAfterDate != null)
            {
                RestRequest.AddQueryParameter("updatedAfterDate",
                    UpdatedAfterDate.Value.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "Z");
            }
            
            RestRequest.AddQueryParameter("includeCompletedTasks", IncludeCompleted.ToString());

            RestRequest.AddHeader("Accept", "application/json");

            IRestResponse response = RestClient.Execute(RestRequest);
            JObject responseObject = JObject.Parse(response.Content);

            return (JArray) responseObject["todo-items"];
        }

        public List<Task> GetTasks()
        {
            List<Task> tasks = new List<Task>();
            JArray rawTasks = GetRawTasks();

            foreach (JObject element in rawTasks)
            {
                Task task = new Task()
                {
                    Id = (long) element["id"],
                    ProjectName = (string) element["project-name"],
                    CompanyName = (string) element["company-name"],
                    Title = (string) element["content"],
                    Description = (string) element["description"],
                    Priority = (string) element["priority"],
                    AssignedTo = (string) element["responsible-party-summary"],
                    Completed = (bool) element["completed"]
                };

                string dateString = (string) element["created-on"];
                DateTime date;
                if (DateTime.TryParse(dateString, out date))
                {
                    task.CreatedOn = date;
                }

                dateString = (string) element["due-date"];
                if (DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out date))
                {
                    task.DueDate = date;
                }
                
                tasks.Add(task);
            }
            
            return tasks;
        }
    }
}