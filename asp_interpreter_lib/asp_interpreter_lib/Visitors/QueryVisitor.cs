using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class QueryVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<Query>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override Query VisitQuery(ASPParser.QueryContext context)
    {
        var literal = context.classical_literal().Accept(new ClassicalLiteralVisitor(_errorLogger));
        
        return new Query(literal);
    }
}