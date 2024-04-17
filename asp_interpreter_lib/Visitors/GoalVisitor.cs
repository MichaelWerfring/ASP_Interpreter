using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class GoalVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<Goal>>
{
    private IErrorLogger _errorLogger = errorLogger;

    public override IOption<Goal> VisitGoal(ASPParser.GoalContext context)
    { 
        return context.children.ElementAt(0).Accept(this);
    }

    public override IOption<Goal> VisitBinary_operation(ASPParser.Binary_operationContext context)
    {
        var visitor = new BinaryOperationVisitor(_errorLogger);

        var result = visitor.VisitBinary_operation(context);
        if (!result.HasValue)
        { 
            errorLogger.LogError("Cannot parse binary operation!", context);
            return new None<Goal>();
        }

        return new Some<Goal>(result.GetValueOrThrow());
    }

    public override IOption<Goal> VisitLiteral(ASPParser.LiteralContext context)
    {
        var visitor = new LiteralVisitor(_errorLogger);

        var result = visitor.VisitLiteral(context);
        if (!result.HasValue) 
        {
            errorLogger.LogError("Cannot parse literal!", context); 
            return new None<Goal>();
        }

        return new Some<Goal>(result.GetValueOrThrow());
    }
}
