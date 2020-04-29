using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using libAPICache.Models;
using libAPICache.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libAPICache.tests.util
{
    [TestClass]
    public class ReflectionHelper
    {
        [TestMethod]
        public void GetEnumerables_WithDateTimeOnly_ShouldReturnEmpty()
        {
            DateTimeOnly input = new DateTimeOnly();

            IEnumerable<PropertyInfo> result = input.GetEnumerables();
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetEnumerables_WithList_ShouldReturnOne()
        {
            WithEnumerable input = new WithEnumerable();
            
            IEnumerable<PropertyInfo> result = input.GetEnumerables();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("date", result.First().Name);
        }

        private class DateTimeOnly : Base
        {
            private DateTime? date { get; set; }
        }

        private class WithEnumerable : Base
        {
            private List<string> Strings { get; set; }
        }
    }
}