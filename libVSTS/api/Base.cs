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
        
        protected CookieContainer CookieContainer { get; set; }
        protected IRestClient RestClient { get; set; }

        protected Base(string apiKey, string organization)
        {
            ApiKey = apiKey;
            Organization = organization;
        }

        protected void CreateClient()
        {
            RestClient = new RestClient($"{BaseURL}/{Organization}");
            RestClient.Authenticator = new HttpBasicAuthenticator("Basic", ApiKey);
            RestClient.CookieContainer = CookieContainer;
        }
    }
}