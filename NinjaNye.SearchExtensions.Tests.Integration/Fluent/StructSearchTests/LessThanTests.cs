using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [TestFixture]
    internal class LessThanTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void LessThan_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => this._context.TestModels.Search(x => x.IntegerOne).LessThan(10));
        }

        [Test]
        public void LessThan_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = this._context.TestModels.Search(x => x.IntegerOne).LessThan(50);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void LessThan_CallWithValue_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = this._context.TestModels.Search(x => x.IntegerOne).LessThan(10);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.IntegerOne < 10));
        }

        [Test]
        public void LessThan_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = this._context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .LessThan(130);

            //Assert
            Assert.IsTrue(result.All(x => x.IntegerOne < 130 || x.IntegerThree < 130));
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}