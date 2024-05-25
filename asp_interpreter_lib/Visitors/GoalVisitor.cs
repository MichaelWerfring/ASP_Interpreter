using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Visitors;

public class GoalVisitor(ILogger logger) : ASPParserBaseVisitor<IOption<Goal>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<Goal> VisitGoal(ASPParser.GoalContext context)
    { 
        return context.children.ElementAt(0).Accept(this);
    }

    public override IOption<Goal> VisitBinary_operation(ASPParser.Binary_operationContext context)
    {
        var visitor = new BinaryOperationVisitor(_logger);

        var result = visitor.VisitBinary_operation(context);
        if (!result.HasValue)
        {
            _logger.LogError("Cannot parse binary operation!", context);
            return new None<Goal>();
        }

        return new Some<Goal>(result.GetValueOrThrow());
    }

    public override IOption<Goal> VisitLiteral(ASPParser.LiteralContext context)
    {
        var visitor = new LiteralVisitor(_logger);

        var result = visitor.VisitLiteral(context);
        if (!result.HasValue) 
        {
            _logger.LogError("Cannot parse literal!", context); 
            return new None<Goal>();
        }

        return new Some<Goal>(result.GetValueOrThrow());
    }
}
