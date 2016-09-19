using System;
using System.Collections.Generic;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests
{
    internal class ParentTestData
    {
        public ICollection<TestData> Children { get; set; }
        public IEnumerable<TestData> OtherChildren { get; set; }
    }

    internal class TestData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }
        public int Age { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    internal class PersonTestData : TestData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }

    }
}