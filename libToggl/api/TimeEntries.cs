namespace libToggl.api
{
    public class TimeEntries : Base
    {
        public TimeEntries(string apiKey) : base(apiKey)
        {
            BaseURL = "";
            _userAgent = "none@nada.com";
        }

        public TimeEntries(string apiKey, string userAgent) : this(apiKey)
        {
            _userAgent = userAgent;
        }
        
        private readonly string _endpointUri = "/details";
        private string _userAgent { get; set; }

    }
}