using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Dommel.Linq.Query
{
    /// <summary>
    /// Represents a query translator which translates expressions to sql queries by deriving from 
    /// <see cref="System.Linq.Expressions.ExpressionVisitor"/>.
    /// </summary>
    public class SqlQueryTranslator : ExpressionVisitor, IQueryTranslator
    {
        private StringBuilder builder;

        public string Translate(Expression expression)
        {
            builder = new StringBuilder();
            Visit(expression);
            return builder.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof (Queryable) &&
                node.Method.Name == "Where")
            {
                Visit(node.Arguments[0]);

                var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
                Visit(lambda.Body);
                return node;
            }

            // todo: parse skip, take, orderby etc.

            throw new NotSupportedException(string.Format("The method '{0}' is not supported.", node.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Not:
                    builder.Append(" not ");
                    Visit(node.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported.", node.NodeType));
            }

            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            builder.Append("(");
            Visit(node.Left);

            switch (node.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    builder.Append(" and ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    builder.Append(" or ");
                    break;
                case ExpressionType.Equal:
                    builder.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    builder.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    builder.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    builder.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    builder.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    builder.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported.", node.NodeType));
            }

            Visit(node.Right);
            builder.Append(")");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var queryable = node.Value as IQueryable;
            if (queryable != null)
            {
                // todo: is the select statement here correct?
                // todo: support projection.
                string tableName = DommelMapper.GetTableName(queryable.ElementType);
                builder.Append("select * from ")
                       .Append(tableName)
                       .Append(" where ");
            }
            else if (node.Value == null)
            {
                builder.Append("null");
            }
            else
            {
                switch (Type.GetTypeCode(node.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        builder.Append((bool)node.Value ? 1 : 0);
                        break;
                    case TypeCode.String:
                        builder.AppendFormat("'{0}'", node.Value);
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported.", node.Value));
                    default:
                        builder.Append(node.Value);
                        break;
                }
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null & node.Expression.NodeType == ExpressionType.Parameter)
            {
                string columnName = DommelMapper.GetColumnName((PropertyInfo)node.Member);
                builder.Append(columnName);
                return node;
            }

            throw new NotSupportedException(string.Format("The member '{0}' is not supported", node.Member.Name));
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }
    }
}
