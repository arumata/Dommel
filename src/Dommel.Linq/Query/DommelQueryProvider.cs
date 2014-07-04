using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        private static readonly MethodInfo _queryMethod;
        private static readonly IDictionary<Type, MethodInfo> _typeMethodInfo = new Dictionary<Type, MethodInfo>();

        static DommelQueryProvider()
        {
            // Find the Query<T>() method in Dapper.
            _queryMethod = typeof (SqlMapper).GetMethods(BindingFlags.Static | BindingFlags.Public)
                                             .First(m => m.Name == "Query" &&
                                                         m.IsGenericMethodDefinition &&
                                                         m.GetGenericArguments().Count() == 1 &&
                                                         m.GetParameters().Count() > 4);
        }

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

            MethodInfo generic;
            if (!_typeMethodInfo.TryGetValue(type, out generic))
            {
                generic = _queryMethod.MakeGenericMethod(new[] { type });
                _typeMethodInfo[type] = generic;
            }

            using (new Profiler("Invoke Query<T> method"))
            {
                // Invoke the generic Query<T> method where T is element type of the expression.
                var result = generic.Invoke(this, new object[] { _connection, query, null, null, true, null, null });
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
