using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dommel.Linq.Utils;

namespace Dommel.Linq.Query
{
    /// <summary>
    /// Represents a query for entity <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class Query<TEntity> : IOrderedQueryable<TEntity>
    {
        private readonly IQueryProvider _queryProvider;
        private readonly Expression _expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dommel.Linq.Query.Query"/> class using the specified <see cref="System.Linq.IQueryProvider"/>.
        /// </summary>
        /// <param name="queryProvider">An implementation of the <see cref="System.Linq.IQueryProvider"/> used for query translation.</param>
        public Query(IQueryProvider queryProvider)
        {
            Check.NotNull(queryProvider, "queryProvider");

            _queryProvider = queryProvider;
            _expression = Expression.Constant(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dommel.Linq.Query.Query"/> class using the specified <see cref="System.Linq.IQueryProvider"/>.
        /// </summary>
        /// <param name="queryProvider">An implementation of the <see cref="System.Linq.IQueryProvider"/> used for query translation.</param>
        /// <param name="expression">The expression representing the query.</param>
        public Query(IQueryProvider queryProvider, Expression expression)
        {
            Check.NotNull(queryProvider, "queryProvider");
            Check.NotNull(expression, "expression");

            if (!typeof (IQueryable<TEntity>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentException("Expression must be of type IQueryable<T>.", "expression");
            }

            _queryProvider = queryProvider;
            _expression = expression;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((IEnumerable<object>)_queryProvider.Execute(_expression)).Select(x => (TEntity)x).GetEnumerator();
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
                return typeof (TEntity);
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
