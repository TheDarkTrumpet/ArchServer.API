using System.Collections.Generic;
using Configuration;
using libAPICache.Abstract;
using libAPICache.util;
using libTeamwork.api;
using Org.BouncyCastle.Math.EC;

namespace libAPICache.Entities
{
    public sealed class EFTeamworkPeople: EFBase<Models.Teamwork.Person, libTeamwork.models.Person>, ITeamworkPeople
    {
        public EFTeamworkPeople() : this(new EFDbContext(), new Config()) { }

        public EFTeamworkPeople(EFDbContext context, IConfig configuration) : base(context, configuration)
        {
            Entries = DbSet = Context.TeamworkPeople;
        }

        public void CacheEntries()
        {
            string apiKey = GetAPIKey("APISources:Teamwork:API_Key");
            string baseURL = GetAPIKey("APISources:Teamwork:Base_URL");
            People people = new People(apiKey, baseURL);

            List<libTeamwork.models.Person> peopleList = people.GetPeople();
            SaveEntries(peopleList);
        }
    }
}