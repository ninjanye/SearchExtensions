using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [TestFixture]
    internal class GreaterThanTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void GreaterThan_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => _context.TestModels.Search(x => x.IntegerOne).GreaterThan(1));
        }

        [Test]
        public void GreaterThan_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = _context.TestModels.Search(x => x.IntegerOne).GreaterThan(50);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GreaterThan_CallWithValue_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne).GreaterThan(10);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.IntegerOne > 10));
        }

        [Test]
        public void GreaterThan_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .GreaterThan(3);

            //Assert
            Assert.IsTrue(result.All(x => x.IntegerOne > 3 || x.IntegerThree > 3));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}