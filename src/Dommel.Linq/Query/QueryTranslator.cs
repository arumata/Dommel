using System.Linq.Expressions;
using System.Text;

namespace Dommel.Linq.Query
{
    public class QueryTranslator : ExpressionVisitor, IQueryTranslator
    {
        private readonly StringBuilder builder = new StringBuilder();

        public string Translate(Expression expression)
        {
            // todo
            return string.Empty;
        }
    }
}
