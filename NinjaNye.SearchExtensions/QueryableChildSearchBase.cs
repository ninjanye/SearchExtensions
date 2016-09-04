using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public abstract class QueryableChildSearchBase<TParent, TChild, TProperty> : ChildSearchBase<TParent, TChild, TProperty>, IQueryable<TParent>
    {
        private readonly IQueryable<TParent> _parent;

        protected QueryableChildSearchBase(IQueryable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParamerter)
            : base(childProperties, properties, completeExpression, childParamerter)
        {
            Expression = parent.Expression;
            ElementType = parent.ElementType;
            Provider = parent.Provider;
            _parent = parent;
        }

        /// <summary>
        /// Begin a search against a new property.
        /// </summary>
        public QueryableChildSearch<TParent, TChild, TAnotherProperty> With<TAnotherProperty>(params Expression<Func<TChild, TAnotherProperty>>[] properties)
        {
            return new QueryableChildSearch<TParent, TChild, TAnotherProperty>(_parent, _childProperties, properties, _completeExpression, _childParameter);            
        }

        /// <summary>
        /// Begin a search against a new string property.
        /// </summary>
        public QueryableChildStringSearch<TParent, TChild> With(params Expression<Func<TChild, string>>[] properties)
        {
            return new QueryableChildStringSearch<TParent, TChild>(_parent, _childProperties, properties, _completeExpression, _childParameter);            
        }
        public IEnumerator<TParent> GetEnumerator()
        {
            var final = BuildFinalExpression();
            return _parent.Where(final).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression { get; private set; }

        public Type ElementType { get; private set; }

        public IQueryProvider Provider { get; private set; }
    }
}