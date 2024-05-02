using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class QueryVisitor(ILogger logger) : ASPBaseVisitor<IOption<Query>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<Query> VisitQuery(ASPParser.QueryContext context)
    {
        var literal = context.literal().Accept(new LiteralVisitor(_logger));

        if (!literal.HasValue)
        {
            return new None<Query>(); 
        }
        
        return new Some<Query>(new Query(literal.GetValueOrThrow()));
    }
}