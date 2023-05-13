using System.Collections.Generic;
using System.Linq;
using Expressions.Task3.E3SQueryProvider.ComplexTask;
using Expressions.Task3.E3SQueryProvider.Models.Entities;
using Xunit;

namespace Expressions.Task3.E3SQueryProvider.Test;

public class PlSqlQueryProviderTests
{
    [Fact]
    public void TestBinaryEqualsQueryable()
    {
        var employees = new List<EmployeeEntity>
        {
            new() { Workstation = "EPRUIZHW006", Manager = "John" },
            new() { Workstation = "EPRUIZHW007", Manager = "Chris" },
        };

        var context = new PlSqlQueryContext();
        var translator = new ExpressionToPlSqlTranslator();

        var query = context
            .Set(employees)
            .Where(e => e.Workstation == "EPRUIZHW006");

        var sqlQuery = translator.Translate(query.Expression);

        Assert.Equal(" WHERE Workstation = EPRUIZHW006", sqlQuery); 
    }
    
    [Fact]
    public void TestSelectQueryable()
    {
        var employees = new List<EmployeeEntity>
        {
            new() { Workstation = "EPRUIZHW006", Manager = "John" },
            new() { Workstation = "EPRUIZHW007", Manager = "Chris" },
        };

        var context = new PlSqlQueryContext();
        var translator = new ExpressionToPlSqlTranslator();

        var query = context
            .Set(employees)
            .Select(e => e.Workstation);

        var sqlQuery = translator.Translate(query.Expression);

        Assert.Equal(" SELECT Workstation", sqlQuery);
    }
}