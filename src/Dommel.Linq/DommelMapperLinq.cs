using System;
using System.Data;
using System.Linq;
using Dommel.Linq.Query;

namespace Dommel.Linq
{
    /// <summary>
    /// Linq operations for Dommel.
    /// </summary>
    public static class DommelMapperLinq
    {
        private static Func<IDbConnection, IQueryTranslator, IQueryProvider> _queryProviderAccessor = (con, translator) => new DommelQueryProvider(con, translator);
        private static IQueryTranslator _queryTranslator = new SqlQueryTranslator();

        public static void SetQueryProviderAccessor(Func<IDbConnection, IQueryTranslator, IQueryProvider> queryProviderAccessor)
        {
            _queryProviderAccessor = queryProviderAccessor;
        }

        public static void SetQueryTranslator(IQueryTranslator queryTranslator)
        {
            _queryTranslator = queryTranslator;
        }

        /// <summary>
        /// Gets a strongly-typed reference to the table in the database allowing further querying.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database.</param>
        /// <returns>A strongly-typed reference to the table in the database.</returns>
        public static IQueryable<TEntity> Table<TEntity>(this IDbConnection connection)
        {
            return new Query<TEntity>(_queryProviderAccessor(connection, _queryTranslator));
        }
    }
}
