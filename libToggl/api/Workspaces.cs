using System;
using System.Collections.Generic;
using libToggl.models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace libToggl.api
{
    public class Workspaces : Base
    {
        public Workspaces(string apiKey) : base(apiKey) { }
        private readonly string _endpointUri = "/workspaces";

        public JArray GetRawWorkspaces()
        {
            RestRequest request = new RestRequest(_endpointUri, Method.GET);
            IRestResponse response = RestClient.Execute(request);
            JArray results = JArray.Parse(response.Content);
            return results;
        }

        public List<Workspace> GetWorkspaces()
        {
            List<Workspace> workspaces = new List<Workspace>();

            JArray resultWorkspaces = GetRawWorkspaces();

            foreach (JObject element in resultWorkspaces)
            {
                workspaces.Add(new Workspace()
                {
                    Id = (int) element["id"],
                    Name = (string) element["name"],
                    Premium = (Boolean.Parse(element["premium"].ToString()))
                });
            }

            return workspaces;
        }
    }
}