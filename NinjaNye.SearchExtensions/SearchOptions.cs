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
            SearchType = SearchExtensions.SearchType.AnyOccurrence;
            ComparisonType = StringComparison.CurrentCulture;
        }

        public bool NullCheck { get; set; }

        public SearchType SearchType { get; set; }

        public StringComparison ComparisonType
        {
            get { return this._comparisonType; }
            set
            {
                this._comparisonType = value;
                _comparisonTypeExpression = Expression.Constant(value);
            }
        }

        public ConstantExpression ComparisonTypeExpression
        {
            get { return _comparisonTypeExpression; }
        }
    }
}