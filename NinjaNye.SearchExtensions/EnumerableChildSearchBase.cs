using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableChildSearchBase<TParent, TChild, TProperty> : ChildSearchBase<TParent, TChild, TProperty>, IEnumerable<TParent>
    {
        private readonly IEnumerable<TParent> _parent;

        protected EnumerableChildSearchBase(IEnumerable<TParent> parent, Expression<Func<TParent, IEnumerable<TChild>>>[] childProperties, Expression<Func<TChild, TProperty>>[] properties, Expression completeExpression, ParameterExpression childParameter)
            : base(childProperties, properties, completeExpression, childParameter)
        {
            _parent = parent;
        }

        /// <summary>
        /// Begin a search against a new property.
        /// </summary>
        public EnumerableChildSearch<TParent, TChild, TAnotherProperty> With<TAnotherProperty>(params Expression<Func<TChild, TAnotherProperty>>[] properties)
        {
            return new EnumerableChildSearch<TParent, TChild, TAnotherProperty>(_parent, _childProperties, properties, _completeExpression, _childParameter);
        }

        /// <summary>
        /// Begin a search against a new string property.
        /// </summary>
        public EnumerableChildStringSearch<TParent, TChild> With(params Expression<Func<TChild, string>>[] properties)
        {
            return new EnumerableChildStringSearch<TParent, TChild>(_parent, _childProperties, properties, _completeExpression, _childParameter);
        }

        public IEnumerator<TParent> GetEnumerator()
        {
            var final = BuildFinalExpression().Compile();
            return _parent.Where(final).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}