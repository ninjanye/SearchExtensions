using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestContext context)
        {
            context.TestModels.AddOrUpdate(tm => tm.Id,
              new TestModel{ Id = new Guid("74EF30BE-A93F-430A-A2BD-FBC31768F5C0"), StringOne = "abcd", StringTwo = "efgh"},
              new TestModel { Id = new Guid("342C35DB-A963-4786-B56B-8D4E83C1EF47"), StringOne = "ijkl", StringTwo = "mnop" },
              new TestModel { Id = new Guid("0C55F838-7BFC-489E-94E3-3EFE3895A503"), StringOne = "qrst", StringTwo = "uvwx" },
              new TestModel { Id = new Guid("0B7216C3-DC4D-4521-AC26-D27D64B57CA0"), StringOne = "yzab", StringTwo = "cdef" },
              new TestModel { Id = new Guid("827D880B-1782-462C-A861-94CC0E180328"), StringOne = "efgh", StringTwo = "ijkl" },
              new TestModel { Id = new Guid("A8A0D984-5F12-4BD2-8E72-9C53D0EA73E0"), StringOne = "UPPER", StringTwo = "CASE" },
              new TestModel { Id = new Guid("3CDF0B28-FFF1-4C0E-B468-E69EF8AAF215"), StringOne = "lower", StringTwo = "case" },
              new TestModel { Id = new Guid("E1DAA440-7EE1-4415-9256-C834351A6329"), StringOne = "ghcdgh", StringTwo = "2 occurences" },
              new TestModel { Id = new Guid("D42FEA7E-C1BA-4875-9E5F-2686E9B079F3"), StringOne = null, StringTwo = "null check" }
            );            
        }
    }
}
