using System;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace libTeamwork.api
{
    public class Tasks : Base
    {
        private readonly string _endpointUri = "/tasks.json";
        public bool IncludeCompleted { get; set; } = true;
        public DateTime? UpdatedAfterDate { get; set; }

        public Tasks(string apiKey, string baseUrl) : base(apiKey, baseUrl)
        {
            CreateClient();
        }

        public JArray GetRawTasks()
        {
            RestRequest request = new RestRequest(_endpointUri, Method.GET);
            request.AddQueryParameter("includeCompletedSubtasks", IncludeCompleted.ToString());

            if (UpdatedAfterDate != null)
            {
                request.AddQueryParameter("updatedAfterDate",
                    UpdatedAfterDate.Value.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "Z");
            }
            request.AddHeader("Accept", "application/json");

            IRestResponse response = RestClient.Execute(request);
            JObject responseObject = JObject.Parse(response.Content);

            return (JArray) responseObject["todo-items"];
        }
    }
}