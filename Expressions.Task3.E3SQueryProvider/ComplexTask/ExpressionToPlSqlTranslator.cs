using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider.ComplexTask;

public class ExpressionToPlSqlTranslator : ExpressionVisitor
{
    private readonly StringBuilder _resultStringBuilder = new StringBuilder();

    public string Translate(Expression exp)
    {
        Visit(exp);
        return _resultStringBuilder.ToString();
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
        {
            _resultStringBuilder.Append(" WHERE ");
            Visit(node.Arguments[1]);
            return node;
        }

        if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Select")
        {
            _resultStringBuilder.Append(" SELECT ");
            Visit(node.Arguments[1]);
            return node;
        }

        throw new NotSupportedException($"Method call '{node.Method.Name}' is not supported");
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);
        switch (node.NodeType)
        {
            case ExpressionType.Equal:
                _resultStringBuilder.Append(" = ");
                break;
            case ExpressionType.NotEqual:
                _resultStringBuilder.Append(" <> ");
                break;
            case ExpressionType.LessThan:
                _resultStringBuilder.Append(" < ");
                break;
            case ExpressionType.LessThanOrEqual:
                _resultStringBuilder.Append(" <= ");
                break;
            case ExpressionType.GreaterThan:
                _resultStringBuilder.Append(" > ");
                break;
            case ExpressionType.GreaterThanOrEqual:
                _resultStringBuilder.Append(" >= ");
                break;
            case ExpressionType.AndAlso:
                _resultStringBuilder.Append(" AND ");
                break;
            case ExpressionType.OrElse:
                _resultStringBuilder.Append(" OR ");
                break;
            default:
                throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
        }

        Visit(node.Right);
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _resultStringBuilder.Append(node.Value);
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression is ParameterExpression)
        {
            _resultStringBuilder.Append(node.Member.Name);
            return base.VisitMember(node);
        }

        throw new NotSupportedException($"The member '{node.Member.Name}' is not supported");
    }
}