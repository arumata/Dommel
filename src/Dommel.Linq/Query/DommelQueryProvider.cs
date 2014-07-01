using System.Data;
using System.Linq.Expressions;

namespace Dommel.Linq.Query
{
    public class DommelQueryProvider : QueryProvider
    {
        private readonly IDbConnection _connection;
        private readonly IQueryTranslator _translator;

        public DommelQueryProvider(IDbConnection connection, IQueryTranslator translator)
        {
            _connection = connection;
            _translator = translator;
        }

        public override object Execute(Expression expression)
        {
            string query = Translate(expression);

            return null;
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
