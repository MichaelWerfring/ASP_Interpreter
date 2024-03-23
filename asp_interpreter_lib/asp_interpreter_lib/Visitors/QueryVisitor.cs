using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class QueryVisitor : ASPBaseVisitor<Query>
{
    public override Query VisitQuery(ASPParser.QueryContext context)
    {
        return new Query(new ClassicalLiteral("a",true, []));
    }
}