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
            this.ElementType = source.ElementType;
            this.Provider = source.Provider;
        }

        public Expression Expression
        {
            get
            {
                this.UpdateSource();
                return this.Source.Expression;
            }
        }

        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }

        protected override void BuildExpression(Expression expressionToJoin)
        {
            this._expressionUpdated = false;
            base.BuildExpression(expressionToJoin);
        }

        private void UpdateSource()
        {
            if (this.CompleteExpression == null || this._expressionUpdated)
            {
                return;
            }

            this._expressionUpdated = true;
            var finalExpression = Expression.Lambda<Func<TSource, bool>>(this.CompleteExpression, this.FirstParameter);
            this.Source = this.Source.Where(finalExpression);
        }

        protected void QueryInclude(string path)
        {
            var includeMethod = Source.GetType().GetMethod("Include", new[] {typeof(string)});
            Source = (IQueryable<TSource>) includeMethod?.Invoke(Source, new[] {path});
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            this.UpdateSource();
            return this.Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}