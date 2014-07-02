using System.Linq.Expressions;

namespace Dommel.Linq.Query
{
    /// <summary>
    /// Defines methods for query translation.
    /// </summary>
    public interface IQueryTranslator
    {
        /// <summary>
        /// Translates the specified query expression.
        /// </summary>
        /// <param name="expression">The query expression.</param>
        /// <returns>The translated query of the expression.</returns>
        string Translate(Expression expression);
    }
}