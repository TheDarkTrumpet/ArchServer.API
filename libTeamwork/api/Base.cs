using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace libTeamwork.api
{
    public class Base
    {
        public string ApiKey { get; protected set; }
        public string BaseURL { get; protected set; }
        
        public string EndPointURI { get; protected set; }
        
        public CookieContainer CookieContainer { get; protected set; }
        public IRestClient RestClient { get; protected set; }
        
        public RestRequest RestRequest { get; protected set; }
        
        protected Base(string apiKey, string baseUrl)
        {
            ApiKey = apiKey;
            BaseURL = baseUrl;
            CookieContainer = new CookieContainer();
        }

        protected void CreateClient()
        {
            RestClient = new RestClient(BaseURL);
            RestClient.Authenticator = new HttpBasicAuthenticator(ApiKey, ApiKey);
            RestClient.CookieContainer = CookieContainer;
        }

        protected void GenerateRestRequest()
        {
            if (string.IsNullOrEmpty(EndPointURI))
            {
                throw new Exception("Unable to create request with empty URI");
            }
            
            RestRequest = new RestRequest(EndPointURI, Method.GET);
            RestRequest.AddHeader("Accept", "application/json");    
        }
    }
}