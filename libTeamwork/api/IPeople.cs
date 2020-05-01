using System.Collections.Generic;
using libTeamwork.models;
using Newtonsoft.Json.Linq;

namespace libTeamwork.api
{
    public interface IPeople
    {
        JArray GetRawPeople();
        List<Person> GetPeople();
    }
}