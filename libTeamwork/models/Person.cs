using System;

namespace libTeamwork.models
{
    public class Person
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public DateTime? LastActive { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyName { get; set; }
        public bool Administrator { get; set; }
    }
}