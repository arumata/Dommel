using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Dommel.Linq.Utils;

namespace Dommel.Linq.Query
{
    public class Query<T> : IOrderedQueryable<T>
    {
        private readonly IQueryProvider _queryProvider;
        private readonly Expression _expression;

        public Query(IQueryProvider queryProvider)
        {
            Check.NotNull(queryProvider, "queryProvider");

            _queryProvider = queryProvider;
            _expression = Expression.Constant(this);
        }

        public Query(IQueryProvider queryProvider, Expression expression)
        {
            Check.NotNull(queryProvider, "queryProvider");
            Check.NotNull(expression, "expression");

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentException("Expression must be of type IQueryable<T>.", "expression");
            }

            _queryProvider = queryProvider;
            _expression = expression;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_queryProvider.Execute(_expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_queryProvider.Execute(_expression)).GetEnumerator();
        }

        public Expression Expression
        {
            get
            {
                return _expression;
            }
        }

        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _queryProvider;
            }
        }
    }
}
