using System.Linq;
using AutoFixture;
using Configuration;
using libAPICache.Entities;
using libTeamwork.api;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libAPICache.tests.Entities
{
    [TestClass]
    public class EFTeamworkPeopleTests : Base<Models.Teamwork.Person, IPeople>
    {
        [TestInitialize]
        public void Initialize()
        {
            Fixture autoFixture = new Fixture();
            IQueryable<People> people = autoFixture.CreateMany<People>(5).AsQueryable();
            
            
        }
    }
}