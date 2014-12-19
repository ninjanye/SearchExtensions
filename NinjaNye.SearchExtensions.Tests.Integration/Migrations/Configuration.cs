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
              new TestModel { Id = new Guid("1675C709-FCF1-4FAD-B664-DC4E199ABC9D"), StringOne = "mouth", StringTwo = "test" },
              new TestModel { Id = new Guid("53684C55-03D5-4A15-9EBA-9DE1A167D99F"), StringOne = "moth", StringTwo = "another" },
              new TestModel { Id = new Guid("278AFE99-F50C-4BA1-B96D-D745299667FD"), StringOne = "plate", StringTwo = "tree" },
              new TestModel { Id = new Guid("7C376836-ACA2-4070-8D22-E8DA98116FBD"), StringOne = "place", StringTwo = "flame" },
              new TestModel { Id = new Guid("BFCEE7CF-12AA-4B09-89EB-5DE127BF9433"), StringOne = "Ashcraft", StringTwo = "white" },
              new TestModel { Id = new Guid("1D875D01-9B03-4F65-8BD6-024C1C5CF515"), StringOne = "ashcroft", StringTwo = "trout" },
              new TestModel { Id = new Guid("CFFF2CC4-FAAB-4D8D-B01A-89B3155799DB"), StringOne = "price", StringTwo = "bloomage" },
              new TestModel { Id = new Guid("F05FF05E-BF52-49CB-A2B2-FF347FE9A4BE"), StringOne = "prize", StringTwo = "flower" },
              new TestModel { Id = new Guid("2B4A8BBD-27D1-437A-A685-F85F1A1DEFF8"), StringOne = "grace", StringTwo = "growth" },
              new TestModel { Id = new Guid("04001511-FB91-44BB-99D8-79A8560BE34C"), StringOne = "graze", StringTwo = "stunt" },
              new TestModel { Id = new Guid("D42FEA7E-C1BA-4875-9E5F-2686E9B079F3"), StringOne = null, StringTwo = "null check" },
              new TestModel { Id = new Guid("AB36C902-5E46-4058-8A61-F3C42659E7FD"), StringOne = "search", StringTwo = "test" },
              new TestModel { Id = new Guid("CF53A4D4-BD9B-468C-8205-E47B9841872E"), StringOne = "search test", StringTwo = "test" },
              new TestModel { Id = new Guid("D42FEA7E-C1BA-4875-9E5F-2686E9B079F3"), StringOne = "windmill", StringTwo = "search" },
              new TestModel { Id = new Guid("86313D62-4847-419D-A0BF-44E80857B880"), StringOne = "search test", StringTwo = "windmill" },
              new TestModel { Id = new Guid("2F75BE28-CEC8-46D8-852E-E6DAE5C8F0A3"), StringOne = "search test", StringTwo = "stringThree", StringThree = "three"},
              new TestModel { Id = new Guid("15598E88-CD58-42E7-A96B-22CB220D5BB2"), StringOne = "search test record match", StringTwo = "search", StringThree = "match"}
            );            
        }
    }
}
