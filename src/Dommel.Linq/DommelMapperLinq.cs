using System;
using System.Data;
using System.Linq;

using Dommel.Linq.Query;

namespace Dommel.Linq
{
    using QueryProvider = Func<IDbConnection, IQueryTranslator, IQueryProvider>;

    public static class DommelMapperLinq
    {
        private static QueryProvider _queryProviderAccessor = (con, translator) => new DommelQueryProvider(con, translator);
        private static IQueryTranslator _queryTranslator = new QueryTranslator();

        public static void SetQueryProviderAccessor(QueryProvider queryProviderAccessor)
        {
            _queryProviderAccessor = queryProviderAccessor;
        }

        public static void SetQueryTranslator(IQueryTranslator queryTranslator)
        {
            _queryTranslator = queryTranslator;
        }

        public static IQueryable<TEntity> Table<TEntity>(this IDbConnection connection)
        {
            return new Query<TEntity>(_queryProviderAccessor(connection, _queryTranslator));
        }
    }
}
