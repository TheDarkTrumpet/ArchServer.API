using System;
using System.Collections.Generic;
using libTeamwork.models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace libTeamwork.api
{
    public class People : Base, IPeople
    {
        public People(string apiKey, string baseUrl) : base(apiKey, baseUrl)
        {
            EndPointURI = "/people.json";
            CreateClient();
            GenerateRestRequest();
        }

        public JArray GetRawPeople()
        {
            IRestResponse response = RestClient.Execute(RestRequest);
            JObject responseObject = JObject.Parse(response.Content);

            return (JArray) responseObject["people"];
        }

        public List<Person> GetPeople()
        {
            List<Person> people = new List<Person>();
            JArray rawPeople = GetRawPeople();

            foreach (JObject element in rawPeople)
            {
                Person person = new Person()
                {
                    Id = (int) element["id"],
                    UserName = (string) element["user-name"],
                    FullName = (string) element["full-name"],
                    EmailAddress = (string) element["email-address"],
                    CompanyName = (string) element["company-name"],
                    Administrator = (bool) element["administrator"]
                };

                if (!String.IsNullOrEmpty(element["last-login"].ToString()))
                {
                    person.LastActive = DateTime.Parse((string) element["last-login"]);
                }
                people.Add(person);
            }

            return people;
        }
    }
}