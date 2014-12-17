using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public class QueryableStringSearchBase<T> : SearchBase<IQueryable<T>, T>, IQueryable<T>
    {
        private bool expressionUpdated;

        protected QueryableStringSearchBase(IQueryable<T> source, Expression<Func<T, string>>[] stringProperties)
            : base(source, stringProperties)
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
            this.expressionUpdated = false;
            base.BuildExpression(expressionToJoin);
        }

        private void UpdateSource()
        {
            if (this.CompleteExpression == null || this.expressionUpdated)
            {
                return;
            }

            this.expressionUpdated = true;
            var finalExpression = Expression.Lambda<Func<T, bool>>(this.CompleteExpression, this.FirstParameter);
            this.Source = this.Source.Where(finalExpression);
        }

        public IEnumerator<T> GetEnumerator()
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