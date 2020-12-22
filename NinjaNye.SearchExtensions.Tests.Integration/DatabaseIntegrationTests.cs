using System;
using NinjaNye.SearchExtensions.Tests.Integration.Migrations;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration
{
    public class DatabaseIntegrationTests : IDisposable
    {
        public readonly TestContext Context;

        public DatabaseIntegrationTests()
        {
            Context = new TestContext();
            Context.Database.EnsureCreated();
            Context.Seed();
        }

        public void Dispose() => Context?.Dispose();
    }

    [CollectionDefinition("Database tests")]
    public class DatabaseCollection : ICollectionFixture<DatabaseIntegrationTests>
    {
        // Intentionally left empty        
    }
}