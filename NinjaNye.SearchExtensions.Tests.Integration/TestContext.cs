using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NinjaNye.SearchExtensions.Tests.Integration.Models;
using Microsoft.Extensions.Logging.Debug;

namespace NinjaNye.SearchExtensions.Tests.Integration
{
    public sealed class TestContext : DbContext
    {
        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<ChildTestModel> ChildTestModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseInMemoryDatabase($"SearchExtensions.Tests.Integration{Guid.NewGuid()}");
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=SearchExtensions.Tests.Integration;Integrated Security=SSPI;TrustServerCertificate=True");
            optionsBuilder.UseLoggerFactory(new LoggerFactory(new []{new DebugLoggerProvider()}));
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChildTestModel>(e => e.HasOne(x => x.Parent)
                .WithMany(x => x.Children).HasForeignKey("Parent_Id"));

            modelBuilder.Entity<ChildTestModel>(e => e.HasOne(x => x.OtherParent)
                .WithMany(x => x.OtherChildren).HasForeignKey("OtherParent_Id"));
        }
    }
}
