using System.Linq.Expressions;

namespace Dommel.Linq.Query
{
    public interface IQueryTranslator
    {
        string Translate(Expression expression);
    }
}