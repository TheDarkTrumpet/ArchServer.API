using System.Collections.Generic;

namespace Configuration
{
    public interface IConfiguration
    {
        string GetKey(string identifier);
        int GetInt(string identifier);
        List<string> GetCollection(string identifier);
    }
}