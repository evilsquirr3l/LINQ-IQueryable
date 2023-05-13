using System.Collections.Generic;
using System.Linq;

namespace Expressions.Task3.E3SQueryProvider.ComplexTask;

public class PlSqlQueryContext
{
    private readonly PlSqlQueryProvider _provider;

    public PlSqlQueryContext()
    {
        _provider = new PlSqlQueryProvider();
    }

    public IQueryable<T> Set<T>(IEnumerable<T> entities)
    {
        return new Query<T>(_provider, entities.AsQueryable().Expression);
    }
}