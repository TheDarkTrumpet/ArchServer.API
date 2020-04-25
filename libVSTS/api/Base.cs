namespace libVSTS.api
{
    public class Base
    {
        protected string ApiKey { get; set; }
        protected string BaseURL { get; set; } = "https://dev.azure.com";
        protected string Organization { get; set; }
        protected string Project { get; set; }
        
        protected CookieContainer CookieContainer { get; set; }
        protected IRestClient RestClient { get; set; }

        protected Base(string apiKey, string organization)
        {
            ApiKey = apiKey;
            organization = Organization;
        }
    }
}