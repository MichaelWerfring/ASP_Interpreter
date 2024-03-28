using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class QueryVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<Query>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<Query> VisitQuery(ASPParser.QueryContext context)
    {
        var literal = context.classical_literal().Accept(new ClassicalLiteralVisitor(_errorLogger));

        if (!literal.HasValue)
        {
            return new None<Query>(); 
        }
        
        return new Some<Query>(new Query(literal.GetValueOrThrow()));
    }
}