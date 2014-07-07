using System;
using System.Data;
using System.Linq.Expressions;
using Dapper;
using Dommel.Linq.Utils;

namespace Dommel.Linq.Query
{
    /// <summary>
    /// Represents a query provider for Dommel.
    /// </summary>
    public class DommelQueryProvider : QueryProvider
    {
        private readonly IDbConnection _connection;
        private readonly IQueryTranslator _translator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dommel.Linq.Query.DommelQueryProvider"/> using the specified 
        /// database connection and query translator.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="translator"></param>
        public DommelQueryProvider(IDbConnection connection, IQueryTranslator translator)
        {
            _connection = connection;
            _translator = translator;
        }

        public override object Execute(Expression expression)
        {
            string query;
            using (new Profiler("Translate"))
            {
                query = Translate(expression);
            }

            Type type = TypeHelper.GetElementType(expression.Type);
            using (new Profiler("Invoke Query<T> method"))
            {
                var result = _connection.Query(type, query);
                return result;
            }
        }

        public override string GetQueryText(Expression expression)
        {
            return Translate(expression);
        }

        private string Translate(Expression expression)
        {
            return _translator.Translate(expression);
        }
    }
}
