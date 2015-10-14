using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [TestFixture]
    internal class SearchChildrenTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test, Ignore]
        public void SearchChild_SearchChildCollection_ReturnParentType()
        {
            //Arrange

            //Act
            var result = this._context.TestModels.Where(x => x.Children.Search(c => c.StringOne).Containing("test").Any());

            //Assert
            Assert.That(result.ToList().All(x => x.StringOne.Contains("test")));
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}