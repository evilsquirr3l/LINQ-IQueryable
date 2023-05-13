using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider;

public class ExpressionToFtsRequestTranslator : ExpressionVisitor
{
    private readonly StringBuilder _resultStringBuilder;

    public ExpressionToFtsRequestTranslator()
    {
        _resultStringBuilder = new StringBuilder();
    }

    public string Translate(Expression exp)
    {
        Visit(exp);

        return _resultStringBuilder.ToString();
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.DeclaringType == typeof(Queryable)
            && node.Method.Name == "Where")
        {
            var predicate = node.Arguments[1];
            Visit(predicate);

            return node;
        }
        return base.VisitMethodCall(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        switch (node.NodeType)
        {
            case ExpressionType.Equal:
                switch (node.Left.NodeType)
                {
                    case ExpressionType.MemberAccess when node.Right.NodeType == ExpressionType.Constant:
                        VisitMemberAndConstant(node.Left, node.Right);
                        break;
                    case ExpressionType.Constant when node.Right.NodeType == ExpressionType.MemberAccess:
                        VisitMemberAndConstant(node.Right, node.Left);
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported operands detected: Left: {node.Left.NodeType}, Right: {node.Right.NodeType}");
                }

                break;

            default:
                throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
        };

        return node;
    }

    private void VisitMemberAndConstant(Expression memberExpression, Expression constantExpression)
    {
        Visit(memberExpression);
        _resultStringBuilder.Append("(");
        Visit(constantExpression);
        _resultStringBuilder.Append(")");
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        _resultStringBuilder.Append(node.Member.Name).Append(":");

        return base.VisitMember(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _resultStringBuilder.Append(node.Value);

        return node;
    }
}