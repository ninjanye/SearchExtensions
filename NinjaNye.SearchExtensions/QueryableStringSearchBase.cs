using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers;

namespace NinjaNye.SearchExtensions
{
    public class QueryableStringSearchBase<T> : IQueryable<T>
    {
        private bool expressionUpdated;
        private Expression completeExpression;
        protected IQueryable<T> Source;
        protected readonly Expression<Func<T, string>>[] StringProperties;
        protected readonly ParameterExpression FirstParameter;

        protected QueryableStringSearchBase(IQueryable<T> source, Expression<Func<T, string>>[] stringProperties)
        {
            this.Source = source;
            this.ElementType = source.ElementType;
            this.Provider = source.Provider;
            this.StringProperties = stringProperties;
            var firstProperty = stringProperties.FirstOrDefault();
            if (firstProperty != null)
            {
                this.FirstParameter = firstProperty.Parameters.FirstOrDefault();
            }
        }

        public Expression Expression
        {
            get
            {
                this.UpdateSource();
                return this.Source.Expression;
            }
        }

        public Type ElementType { get; protected set; }
        public IQueryProvider Provider { get; protected set; }

        protected void AppendExpression(Expression expressionToJoin)
        {
            this.expressionUpdated = false;
            this.completeExpression = ExpressionHelper.JoinAndAlsoExpression(this.completeExpression, expressionToJoin);
        }

        private void UpdateSource()
        {
            if (this.completeExpression == null || this.expressionUpdated)
            {
                return;
            }

            this.expressionUpdated = true;
            var finalExpression = Expression.Lambda<Func<T, bool>>(this.completeExpression, this.FirstParameter);
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