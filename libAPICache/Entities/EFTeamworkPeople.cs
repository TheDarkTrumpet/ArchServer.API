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
        private IPeople _iPeople;
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public EFTeamworkPeople() : this(new EFDbContext(), new Config()) { }

        public EFTeamworkPeople(EFDbContext context, IConfig configuration, IPeople people = null) : base(context, configuration)
        {
            Entries = DbSet = Context.TeamworkPeople;
            
            ApiKey = Configuration.GetKey("APISources:Teamwork:API_Key");
            BaseUrl = Configuration.GetKey("APISources:Teamwork:Base_URL");
            
            _iPeople = people ?? new People(ApiKey, BaseUrl);
        }

        public void CacheEntries()
        {
            List<libTeamwork.models.Person> peopleList = _iPeople.GetPeople();
            SaveEntries(peopleList);
        }
    }
}