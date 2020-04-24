namespace libTeamwork.api
{
    public class Base
    {
        protected string ApiKey { get; set; }
        protected string BaseURL { get; set; }
        
        protected CookieContainer CookieContainer { get; set; }
        protected IRestClient RestClient { get; set; }
        
        protected Base(string apiKey)
        {
            ApiKey = apiKey;
            BaseURL = "https://www.toggl.com/api/v8";
            CookieContainer = new CookieContainer();
        }

        protected void CreateClient()
        {
            RestClient = new RestClient(BaseURL);
            RestClient.Authenticator = new HttpBasicAuthenticator(ApiKey, "api_token");
            RestClient.CookieContainer = CookieContainer;
        }
    }
}