using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace libVSTS.api
{
    public class Base
    {
        public string ApiKey { get; set; }
        public string BaseURL { get; set; } = "https://dev.azure.com";
        public string Organization { get; set; }
        public string Project { get; set; }
        public string EndPointUri { get; set; }
        
        protected CookieContainer CookieContainer { get; set; }
        protected IRestClient RestClient { get; set; }
        protected RestRequest RestRequest { get; set; }

        protected Base(string apiKey, string organization, string project)
        {
            ApiKey = apiKey;
            Organization = organization;
            Project = project;
            EndPointUri = $"{Project}/_apis/wit/wiql";
            CreateClient();
            CreateRestRequest();
        }

        protected void CreateClient()
        {
            if (string.IsNullOrEmpty(BaseURL) || string.IsNullOrEmpty(Organization))
            {
                throw new Exception("Unable to create a client if either the BaseURL or the Organization is null");
            }
            RestClient = new RestClient($"{BaseURL}/{Organization}");
            RestClient.Authenticator = new HttpBasicAuthenticator("Basic", ApiKey);
            RestClient.CookieContainer = CookieContainer;
        }

        protected void CreateRestRequest(string uri = null, RestSharp.Method method = Method.GET)
        {
            if (string.IsNullOrEmpty(EndPointUri) && String.IsNullOrEmpty(uri))
            {
                throw new Exception(
                    "Unable to create a rest request if the endpointuri is not defined, or the url passed in is not included");
            }

            uri ??= EndPointUri;
            RestRequest = new RestRequest(uri, method);
            
        }
    }
}