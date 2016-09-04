using System;
using System.Collections.Generic;

namespace NinjaNye.SearchExtensions.Tests.Integration.Models
{
    public class TestModel
    {
        private readonly DateTime _defaultDate = new DateTime(1970, 1, 1);

        public TestModel()
        {            
            Start = _defaultDate;
            End = _defaultDate;
        }

        public virtual ICollection<ChildTestModel> Children { get; set; }
        public virtual ICollection<ChildTestModel> OtherChildren { get; set; }

        public Guid Id { get; set; }
        public string StringOne { get; set; }
        public string StringTwo { get; set; }
        public string StringThree { get; set; }
        public int IntegerOne { get; set; }
        public int IntegerTwo { get; set; }
        public int IntegerThree { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
