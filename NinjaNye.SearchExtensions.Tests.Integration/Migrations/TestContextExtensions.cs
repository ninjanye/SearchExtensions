using System.Collections.Generic;
using System.Linq;
using NinjaNye.SearchExtensions.Tests.Integration.Models;

namespace NinjaNye.SearchExtensions.Tests.Integration.Migrations
{
    using System;

    internal static class TestContextExtensions
    {
        public static void Seed(this TestContext context)
        {
            if (context.TestModels.Any())
            {
                return;
            }

            context.TestModels.AddRange(new[] {
                new TestModel {Id = new Guid("74EF30BE-A93F-430A-A2BD-FBC31768F5C0"), StringOne = "abcd", StringTwo = "efgh"},
                new TestModel {Id = new Guid("342C35DB-A963-4786-B56B-8D4E83C1EF47"), StringOne = "ijkl", StringTwo = "mnop"},
                new TestModel {Id = new Guid("0C55F838-7BFC-489E-94E3-3EFE3895A503"), StringOne = "qrst", StringTwo = "uvwx"},
                new TestModel {Id = new Guid("0B7216C3-DC4D-4521-AC26-D27D64B57CA0"), StringOne = "yzab", StringTwo = "cdef"},
                new TestModel {Id = new Guid("827D880B-1782-462C-A861-94CC0E180328"), StringOne = "efgh", StringTwo = "ijkl"},
                new TestModel {Id = new Guid("A8A0D984-5F12-4BD2-8E72-9C53D0EA73E0"), StringOne = "UPPER", StringTwo = "CASE"},
                new TestModel {Id = new Guid("3CDF0B28-FFF1-4C0E-B468-E69EF8AAF215"), StringOne = "lower", StringTwo = "case"},
                new TestModel {Id = new Guid("E1DAA440-7EE1-4415-9256-C834351A6329"), StringOne = "ghcdgh", StringTwo = "2 occurences"},
                new TestModel {Id = new Guid("1675C709-FCF1-4FAD-B664-DC4E199ABC9D"), StringOne = "mouth", StringTwo = "test"},
                new TestModel {Id = new Guid("53684C55-03D5-4A15-9EBA-9DE1A167D99F"), StringOne = "moth", StringTwo = "another"},
                new TestModel {Id = new Guid("278AFE99-F50C-4BA1-B96D-D745299667FD"), StringOne = "plate", StringTwo = "tree"},
                new TestModel {Id = new Guid("7C376836-ACA2-4070-8D22-E8DA98116FBD"), StringOne = "place", StringTwo = "flame"},
                new TestModel {Id = new Guid("BFCEE7CF-12AA-4B09-89EB-5DE127BF9433"), StringOne = "Ashcraft", StringTwo = "white"},
                new TestModel {Id = new Guid("1D875D01-9B03-4F65-8BD6-024C1C5CF515"), StringOne = "ashcroft", StringTwo = "trout"},
                new TestModel {Id = new Guid("CFFF2CC4-FAAB-4D8D-B01A-89B3155799DB"), StringOne = "price", StringTwo = "bloomage"},
                new TestModel {Id = new Guid("F05FF05E-BF52-49CB-A2B2-FF347FE9A4BE"), StringOne = "prize", StringTwo = "flower"},
                new TestModel {Id = new Guid("2B4A8BBD-27D1-437A-A685-F85F1A1DEFF8"), StringOne = "grace", StringTwo = "growth"},
                new TestModel {Id = new Guid("04001511-FB91-44BB-99D8-79A8560BE34C"), StringOne = "graze", StringTwo = "stunt"},
                new TestModel {Id = new Guid("D42FEA7E-C1BA-4875-9E5F-2686E9B079F3"), StringOne = null, StringTwo = "null check"},
                new TestModel {Id = new Guid("AB36C902-5E46-4058-8A61-F3C42659E7FD"), StringOne = "search", StringTwo = "test"},
                new TestModel {Id = new Guid("CF53A4D4-BD9B-468C-8205-E47B9841872E"), StringOne = "search test", StringTwo = "test"},
                new TestModel {Id = new Guid("990E97D8-F1DD-4B40-B85F-60075FABCD4C"), StringOne = "windmill", StringTwo = "search"},
                new TestModel {Id = new Guid("89ACBC75-ABEB-4064-88FF-8F18643B87DA"), StringOne = "test", StringTwo = "test"},
                new TestModel {Id = new Guid("02DE65DE-53B3-4DB7-B6A1-2BF90F5F6A5A"), StringOne = "three", StringTwo = "test", StringThree = "test"},
                new TestModel {Id = new Guid("624CFA9C-4FA1-4680-880D-AAB6507A3014"), StringOne = "three", StringTwo = "test", StringThree = "three"},
                new TestModel {Id = new Guid("86313D62-4847-419D-A0BF-44E80857B880"), StringOne = "search test", StringTwo = "windmill", StringThree = "wind"},
                new TestModel {Id = new Guid("2F75BE28-CEC8-46D8-852E-E6DAE5C8F0A3"), StringOne = "search test", StringTwo = "stringThree", StringThree = "search"},
                new TestModel {Id = new Guid("15598E88-CD58-42E7-A96B-22CB220D5BB2"), StringOne = "search test record", StringTwo = "search", StringThree = "record"},
                new TestModel {Id = new Guid("A8AD8A4F-853B-417A-9C0C-0A2802560B8C"), StringOne = "wholewordmatch", StringTwo = "match", StringThree = "whole"},
                new TestModel {Id = new Guid("CADA7A13-931A-4CF0-B4F4-49160A743251"), StringOne = "whole word match", StringTwo = "match", StringThree = "whole"},
                new TestModel {Id = new Guid("DA60DD08-F3A6-4709-9560-AAFDAA91F3EB"), Start = new DateTime(2000, 1, 1), End = new DateTime(2010, 1, 1)},
                new TestModel {Id = new Guid("D38974C3-72B5-44E6-B54E-CFF41552DE90"), Start = new DateTime(2010, 1, 1), End = new DateTime(2020, 1, 1)},
                new TestModel {Id = new Guid("C24AD0A7-5B57-40D1-B5CC-FE8F0082FA78"), Start = new DateTime(2020, 1, 1), End = new DateTime(2030, 1, 1)},
                new TestModel {Id = new Guid("35BC5401-AF48-4757-A01D-1A5E85D926ED"), Start = new DateTime(2030, 1, 1), End = new DateTime(2040, 1, 1)},
                new TestModel {Id = new Guid("8AC7CFF4-0BF6-4883-95FB-6729369E606D"), Start = new DateTime(2040, 1, 1), End = new DateTime(2050, 1, 1)},
                new TestModel {Id = new Guid("2F2DBCB1-88D0-4E95-84AF-1CE073C03513"), IntegerOne = 50, IntegerTwo = 0, IntegerThree = 200 },
                new TestModel {Id = new Guid("CAEF2CD5-1AED-4BEE-A5E7-36A711A1ABF3"), IntegerOne = 101, IntegerTwo = 201, IntegerThree = 301 },
                new TestModel {Id = new Guid("477BFDFB-5EEA-411D-B9AB-8E7F6DCB5BD3"), IntegerOne = 3, IntegerTwo = 2, IntegerThree = 1 },
                new TestModel
                {
                    Id = new Guid("F672552D-2787-468D-8D2E-DE1E88F83E21"), StringOne = "parent test model", StringTwo = "parent",
                    Children = new List<ChildTestModel>
                    {
                        new() {Id = new Guid("ADA68177-DE83-4462-B6F7-81827F9D8EA8"), StringOne = "child test", StringTwo = "child one"},
                        new() {Id = new Guid("78AF04DC-A8AA-4293-9F9F-582A93C717BF"), StringOne = "test child", StringTwo = "child two"},
                        new() {Id = new Guid("CCCF9D7B-631A-43D8-AC7A-58C6061890A8"), IntegerOne = 50, IntegerTwo = 0, IntegerThree = 200, StringOne = "child test", StringTwo = "child one"},
                        new() {Id = new Guid("EA2FEBC5-6689-4B15-8528-B8E86EA064F1"), IntegerOne = 101, IntegerTwo = 201, IntegerThree = 301, StringOne = "child test", StringTwo = "child two"},
                        new() {Id = new Guid("2191405A-6269-440F-8DD9-000882D742A3"), IntegerOne = 3, IntegerTwo = 2, IntegerThree = 1, StringOne = "child test", StringTwo = "child three"}
                    },
                    OtherChildren = new List<ChildTestModel>
                    {
                        new() {Id = new Guid("801E3EA7-2714-44AB-AEBA-AD94BB7A79C9"), StringOne = "another child", StringTwo = "child three"},
                        new() {Id = new Guid("FCF9D38B-A16D-4FF0-9491-972ED6A3FDDB"), StringOne = "more children", StringTwo = "child four"},
                        new() {Id = new Guid("33039698-541B-4A34-A474-449356009D03"), IntegerOne = 1, IntegerTwo = 2, IntegerThree = 3, StringOne = "another child", StringTwo = "child five"},
                        new() {Id = new Guid("1AF72E86-194B-4B43-81BD-C902786F782B"), IntegerOne = 101, IntegerTwo = 102, IntegerThree = 200, StringOne = "another child", StringTwo = "child six"},
                        new() {Id = new Guid("5D7E7F77-4459-4EE1-9863-E76D467254C2"), IntegerOne = 101, IntegerTwo = 0, IntegerThree = 200, StringOne = "another child", StringTwo = "child one"}
                    }
                },
                new TestModel
                {
                    Id = new Guid("24726ECC-953E-4F95-88AA-91E0C0B52D00"), StringOne = "parent model test", StringTwo = "parent",
                    Children = new List<ChildTestModel>
                    {
                        new() {Id = new Guid("BAD85289-DCC5-48FD-A054-58360A86F9FC"), StringOne = "another child", StringTwo = "child three"},
                        new() {Id = new Guid("DA7B9641-D554-49A1-9F0F-3E6988733D1B"), StringOne = "more children", StringTwo = "child four"},
                        new() {Id = new Guid("9FD0D2A9-BC71-42A3-9D72-8AA72619AA60"), IntegerOne = 1, IntegerTwo = 2, IntegerThree = 3, StringOne = "another child", StringTwo = "child five"},
                        new() {Id = new Guid("697AFA70-6544-4475-BB3F-A5FF4672B063"), IntegerOne = 101, IntegerTwo = 102, IntegerThree = 200, StringOne = "another child", StringTwo = "child six"},
                        new() {Id = new Guid("2D546C94-BB00-45EA-9C9B-76837CC823EC"), IntegerOne = 101, IntegerTwo = 0, IntegerThree = 200, StringOne = "another child", StringTwo = "child one"}
                    },
                    OtherChildren = new List<ChildTestModel>
                    {
                        new() {Id = new Guid("619F6AA2-1A19-49A3-81E2-21ED228B4913"), StringOne = "child test", StringTwo = "child one"},
                        new() {Id = new Guid("3D097152-38C4-4DE8-B020-C03FFCFC5090"), StringOne = "test child", StringTwo = "child two"},
                        new() {Id = new Guid("3D8EEE31-28BB-446C-9F91-0E971327BD0A"), IntegerOne = 50, IntegerTwo = 0, IntegerThree = 200, StringOne = "child test", StringTwo = "child one"},
                        new() {Id = new Guid("09C25079-CCD8-41F4-BAC1-72BF84B59CBB"), IntegerOne = 101, IntegerTwo = 201, IntegerThree = 301, StringOne = "child test", StringTwo = "child two"},
                        new() {Id = new Guid("6312B038-1FCA-4746-A168-7DB45E45057A"), IntegerOne = 3, IntegerTwo = 2, IntegerThree = 1, StringOne = "child test", StringTwo = "child three"}
                    }
                }
            });

            context.SaveChanges();
        }
    }
}
