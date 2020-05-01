using System.Collections.Generic;
using libAPICache.Abstract;
using libAPICache.util;
using libTeamwork.api;

namespace libAPICache.Entities
{
    public sealed class EFTeamworkPeople: EFBase<Models.Teamwork.Person, libTeamwork.models.Person>, ITeamworkPeople
    {
        public EFTeamworkPeople() : this(new EFDbContext(), new Configuration()) { }

        public EFTeamworkPeople(EFDbContext context, Configuration configuration) : base(context, configuration)
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