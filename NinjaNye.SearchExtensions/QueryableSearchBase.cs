using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public class QueryableSearchBase<TSource, TProperty> 
        : SearchBase<IQueryable<TSource>, TSource, TProperty>, IQueryable<TSource>
    {
        private bool _expressionUpdated;

        protected QueryableSearchBase(IQueryable<TSource> source, Expression<Func<TSource, TProperty>>[] properties)
            : base(source, properties)
        {
            ElementType = source.ElementType;
            Provider = source.Provider;
        }

        public Expression Expression
        {
            get
            {
                UpdateSource();
                return Source.Expression;
            }
        }

        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }

        protected override void BuildExpression(Expression expressionToJoin)
        {
            _expressionUpdated = false;
            base.BuildExpression(expressionToJoin);
        }

        private void UpdateSource()
        {
            if (CompleteExpression == null || _expressionUpdated)
            {
                return;
            }

            _expressionUpdated = true;
            var finalExpression = Expression.Lambda<Func<TSource, bool>>(CompleteExpression, FirstParameter);
            Source = Source.Where(finalExpression);
        }

        protected void QueryInclude(string path)
        {
            var includeMethod = Source.GetType().GetMethod("Include", new[] {typeof(string)});
            Source = (IQueryable<TSource>) includeMethod?.Invoke(Source, new[] {path});
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            UpdateSource();
            return Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}