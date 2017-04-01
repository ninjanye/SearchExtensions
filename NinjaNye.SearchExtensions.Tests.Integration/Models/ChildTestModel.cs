using System;

namespace NinjaNye.SearchExtensions.Tests.Integration.Models
{
    public class ChildTestModel
    {
        public Guid Id { get; set; }
        public string StringOne { get; set; }
        public string StringTwo { get; set; }
        public string StringThree { get; set; }
        public int IntegerOne { get; set; }
        public int IntegerTwo { get; set; }
        public int IntegerThree { get; set; }

        public TestModel Parent { get; set; }
        public TestModel OtherParent { get; set; }
    }
}