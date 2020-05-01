using Configuration;
using libAPICache.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace libAPICache.tests.Entities
{
    public class Base<T, T1> where T: class where T1: class
    {
        protected Mock<EFDbContext> _context;
        protected Mock<DbSet<T>> _mockDbSet;
        protected Mock<IConfig> _config;
        protected Mock<T1> _iAPIMethod;
    }
}