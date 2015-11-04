using System.Data.Entity;
using NinjaNye.SearchExtensions.Tests.Integration.Migrations;
using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration
{
    public class TestContext : DbContext
    {
        public TestContext()
            : base("TestConnection")
        {
        }

        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<ChildTestModel> ChildTestModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TestContext, Configuration>());
            base.OnModelCreating(modelBuilder);
        }
    }


}
