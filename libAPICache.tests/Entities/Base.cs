using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Configuration;
using libAPICache.Entities;
using libAPICache.Models;
using libAPICache.tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace libAPICache.tests.Entities
{
    public class Base<T, T1> where T: Base, new() where T1: class
    {
        protected Mock<EFDbContext> _context;
        protected Mock<DbSet<T>> _mockDbSet;
        protected Mock<IConfig> _config;
        protected Mock<T1> _iAPIMethod;

        protected void Setup(IQueryable<T> entries = null)
        {
            if (entries == null)
            {
                entries = GenerateFixtures();
            }
            
            _mockDbSet = GenerateDBSetHelper<T>.GenerateDbSet(entries);

            _context = new Mock<EFDbContext>();
            _iAPIMethod = new Mock<T1>();
            _config = new Mock<IConfig>();
        }

        protected T GenerateFixture()
        {
            Fixture autoFixture = new Fixture();
            return autoFixture.Create<T>();
        }

        protected IQueryable<T> GenerateFixtures()
        {
            Fixture autoFixture = new Fixture();
            IQueryable<T> entries = autoFixture.CreateMany<T>().AsQueryable();
            return entries;
        }
    }
}