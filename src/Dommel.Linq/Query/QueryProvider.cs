using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Dommel.Linq.Utils;

namespace Dommel.Linq.Query
{
    /// <summary>
    /// Serves as the base class for all query providers for the <see cref="Dommel.Linq.Query.Query"/> class.
    /// </summary>
    public abstract class QueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeHelper.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new Query<TElement>(this, expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Execute(expression);
        }

        public abstract object Execute(Expression expression);

        /// <summary>
        /// Gets the translated text for the specified query.
        /// </summary>
        /// <param name="expression">The expression to translate.</param>
        /// <returns>The translated text for the query.</returns>
        public abstract string GetQueryText(Expression expression);
    }
}
