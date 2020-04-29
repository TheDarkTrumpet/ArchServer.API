using System;
using System.Collections.Generic;
using AutoFixture;
using libAPICache.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libAPICache.tests.util
{
    [TestClass]
    public class CopyToTests
    {
        private CopyToTestsModel _expected = new CopyToTestsModel();
        
        [TestInitialize]
        public void Setup()
        {
            _expected = new CopyToTestsModel()
            {
                Id = 1,
                Name = "Test Name",
                Date = DateTime.Now,
                Strings = new List<string>()
                {
                    "stringA",
                    "stringB"
                }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _expected = null;
        }
        
        [TestMethod]
        public void CopyTo_WithNonEnumerables_ShouldCopy()
        {
            CopyToTestsModel result = new CopyToTestsModel();
            result.Copy(_expected);

            Assert.AreEqual(_expected.Id, result.Id);
            Assert.AreEqual(_expected.Name, result.Name);
            Assert.AreEqual(_expected.Date, result.Date);
        }

        [TestMethod]
        public void CopyTo_WithEnumerables_ShouldNotCopyThem()
        {
            CopyToTestsModel result = new CopyToTestsModel();
            result.Copy(_expected);
            
            Assert.IsNull(result.Strings);
        }

        private class CopyToTestsModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime? Date { get; set; }
            public IEnumerable<string> Strings { get; set; }
        }
    }
}