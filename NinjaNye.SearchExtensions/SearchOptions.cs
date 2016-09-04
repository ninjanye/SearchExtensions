using System;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public class SearchOptions
    {
        private StringComparison _comparisonType;
        private ConstantExpression _comparisonTypeExpression;

        public SearchOptions()
        {
            NullCheck = true;
            SearchType = SearchType.AnyOccurrence;
            ComparisonType = StringComparison.CurrentCulture;
        }

        public bool NullCheck { get; private set; }

        public SearchType SearchType { get; set; }

        public StringComparison ComparisonType
        {
            get { return _comparisonType; }
            set
            {
                _comparisonType = value;
                _comparisonTypeExpression = Expression.Constant(value);
            }
        }

        public ConstantExpression ComparisonTypeExpression => _comparisonTypeExpression;
    }
}