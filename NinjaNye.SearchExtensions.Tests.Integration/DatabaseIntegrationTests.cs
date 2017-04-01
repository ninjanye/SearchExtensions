using System;
using NinjaNye.SearchExtensions.Tests.Integration.Migrations;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration
{
    public class DatabaseIntegrationTests : IDisposable
    {
        public readonly TestContext _context;

        public DatabaseIntegrationTests()
        {
            _context = new TestContext();
            _context.Database.EnsureCreated();
            _context.Seed();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    [CollectionDefinition("Database tests")]
    public class DatabaseCollection : ICollectionFixture<DatabaseIntegrationTests>
    {
        // Intentionally left empty        
    }
}