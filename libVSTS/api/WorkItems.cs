using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace libVSTS.api
{
    public class WorkItems : Base
    {
        private readonly string _endpointUri = "/_apis/wit/wiql";
        public List<string> StatesToExclude { get; set; }
        public List<string> AssignedToInclude { get; set; }
        public List<string> TypesToInclude { get; set; }
        public DateTime? FromChanged { get; set; }
        
        public WorkItems(string apiKey, string organization) : base(apiKey, organization)
        {
            StatesToExclude = new List<string>();
            AssignedToInclude = new List<string>();
            TypesToInclude = new List<string>();
            CreateClient();
        }

        public JArray GetRawWorkItems()
        {
            RestRequest request = new RestRequest("/PharElectronicSyllabus/_apis/wit/wiql", Method.POST);
            request.AddQueryParameter("api-version", "5.1");
            request.AddQueryParameter("$depth", "2");
            request.AddQueryParameter("$expand", "all");
            request.AddHeader("Content-Type", "application/json");
            
            
        }
    }
}