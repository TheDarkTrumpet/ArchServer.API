using System.Collections.Generic;
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
        [TestMethod]
        public void EFTeamworkPeople_WithProperties_ShouldSet()
        {
            EFTeamworkPeople efTeamworkPeople =
                new EFTeamworkPeople(_context.Object, _config.Object, _iAPIMethod.Object);
            
            Assert.IsNotNull(efTeamworkPeople.Entries);
            Assert.IsNotNull(efTeamworkPeople.ApiKey);
            Assert.IsNotNull(efTeamworkPeople.BaseUrl);
        }

        [TestMethod]
        public void CacheEntries_ShouldGetPeopleAndCallSave()
        {
            EFTeamworkPeople efTeamworkPeople =
                new EFTeamworkPeople(_context.Object, _config.Object, _iAPIMethod.Object);
            
            efTeamworkPeople.CacheEntries();
            
            _iAPIMethod.Verify(x => x.GetPeople(), Times.Once);
            _context.Verify(x => x.SaveChanges(), Times.Once);
        }
        
        [TestInitialize]
        public void Initialize()
        {
            Setup();

            _context.Setup(x => x.TeamworkPeople).Returns(_mockDbSet.Object);

            _iAPIMethod.Setup(x => x.GetPeople()).Returns(new List<libTeamwork.models.Person>());

            _config.Setup(x => x.GetKey("APISources:Teamwork:API_Key")).Returns("An API Key");
            _config.Setup(x => x.GetKey("APISources:Teamwork:Base_URL")).Returns("A Base URL");
        }
    }
}