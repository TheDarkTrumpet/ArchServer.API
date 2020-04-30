using System.Collections.Generic;
using System.Linq;
using libAPICache.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace libAPICache.tests.Helpers
{
    public static class GenerateDBSetHelper<T> where T: Base, new()
    {
        public static Mock<DbSet<T>> GenerateDbSet(IQueryable<T> elements)
        {
            Mock<DbSet<T>> mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elements.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elements.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elements.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
                .Returns(elements.GetEnumerator());

            return mockSet;
        }
    }
}