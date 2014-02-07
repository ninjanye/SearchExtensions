using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class SearchTests
    {
        private readonly List<string> enumerableData = new List<string>();

        [TestFixtureSetUp]
        public void ClassSetup()
        {
            for (int i = 0; i < 10; i++)
            {
                Guid guid = Guid.NewGuid();
                enumerableData.Add(guid.ToString());
            }
        }

        [Test]
        public void Search_SearchTermNotSupplied_AllDataReturned()
        {
            //Arrange
            
            //Act
            var result = enumerableData.Search((string)null, s => s);

            //Assert
            Assert.AreEqual(enumerableData, result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_SearchPropertyNotSupplied_ThrowsArgumentNullException()
        {
            //Arrange
            
            //Act
            enumerableData.Search("test", (Expression<Func<string, string>>)null);

            //Assert
            Assert.Fail("Expected an Argument Null expception to occur");
        }

        [Test]
        public void Search_SearchAllProperties_SearchesAllProperties()
        {
            //Arrange
            var data = new List<TestClass>();
            data.Add(new TestClass{Name = "abcd", Description = "efgh"});
            data.Add(new TestClass{Name = "ijkl", Description = "mnop"});
            data.Add(new TestClass{Name = "qrst", Description = "uvwx"});
            data.Add(new TestClass{Name = "yz", Description = "abcd"});
            
            //Act
            var result = data.AsQueryable().Search("b");

            //Assert
            Assert.AreEqual(2, result.Count());

        }

        private class TestClass
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

    }
}