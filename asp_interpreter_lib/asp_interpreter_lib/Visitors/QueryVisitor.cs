using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class QueryVisitor : ASPBaseVisitor<Query>
{
    public override Query VisitQuery(ASPParser.QueryContext context)
    {
        var literal = context.classical_literal().Accept(new ClassicalLiteralVisitor());
        
        return new Query(literal);
    }
}