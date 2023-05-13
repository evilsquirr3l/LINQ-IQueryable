using System;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions.Task3.E3SQueryProvider.ComplexTask;

public class PlSqlQueryProvider : IQueryProvider
{
    public IQueryable CreateQuery(Expression expression)
    {
        return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(expression.Type), this,
            expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new Query<TElement>(this, expression);
    }

    public object Execute(Expression expression)
    {
        return Execute(expression, false);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return (TResult)Execute(expression, typeof(TResult).Name == "IEnumerable`1");
    }

    public string GetQueryText(Expression expression)
    {
        return Translate(expression);
    }

    private object Execute(Expression expression, bool isEnumerable)
    {
        throw new NotImplementedException();
    }

    private string Translate(Expression expression)
    {
        var translator = new ExpressionToPlSqlTranslator();
        return translator.Translate(expression);
    }
}