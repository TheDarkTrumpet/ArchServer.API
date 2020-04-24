using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace libTeamwork.api
{
    public class Base
    {
        protected string ApiKey { get; set; }
        protected string BaseURL { get; set; }
        
        protected CookieContainer CookieContainer { get; set; }
        protected IRestClient RestClient { get; set; }
        
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