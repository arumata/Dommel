using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Dommel.Linq.Utils;

namespace Dommel.Linq.Query
{
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

        public abstract string GetQueryText(Expression expression);
    }
}
