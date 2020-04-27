using System.Collections.Generic;
using libAPICache.Abstract;
using libTeamwork.api;

namespace libAPICache.Entities
{
    public class EFTeamworkPeople: EFBase<Models.Teamwork.Person, libTeamwork.models.Person>, ITeamworkPeople
    {
        public EFTeamworkPeople() : this(new EFDbContext()) { }

        public EFTeamworkPeople(EFDbContext context) : base(context)
        {
            Entries = _dbSet = _context.TeamworkPeople;
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