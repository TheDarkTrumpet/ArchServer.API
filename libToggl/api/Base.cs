using System;

namespace libToggl.api
{
    public class Base
    {
        protected string ApiKey { get; set; }
        protected string BaseURL { get; set; }

        public Base(string apiKey)
        {
            ApiKey = apiKey;
            BaseURL = "https://www.toggl.com/api/v8";
        }
    }
}
