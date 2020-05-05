using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace libTeamwork.api
{
    public class Base
    {
        public string ApiKey { get; private set; }
        public string BaseURL { get; private set; }
        
        public CookieContainer CookieContainer { get; private set; }
        public IRestClient RestClient { get; private set; }
        
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
    }
}