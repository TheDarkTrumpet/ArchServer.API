﻿using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace libToggl.api
{
    public class Base : IBase
    {
        public string ApiKey { get; set; }
        public string BaseURL { get; set; }
        protected string BaseUri { get; set; }
        
        public CookieContainer CookieContainer { get; protected set; }
        public IRestClient RestClient { get; protected set; }
        public RestRequest RestRequest { get; protected set; }
        
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

        protected void GenerateRestRequest()
        {
            if (string.IsNullOrEmpty(BaseUri))
            {
                throw new Exception("Unable to create request with a missing URI");
            }
            RestRequest = new RestRequest(BaseUri, Method.GET);
        }
    }
}
