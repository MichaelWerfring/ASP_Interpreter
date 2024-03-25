using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.BinaryOperations;

namespace asp_interpreter_lib.Visitors;

public class BinaryOperatorVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<BinaryOperator>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override BinaryOperator VisitEqualityOperation(ASPParser.EqualityOperationContext context)
    {
        return new Equality();
    }

    public override BinaryOperator VisitDisunificationOperation(ASPParser.DisunificationOperationContext context)
    {
        return new Disunification();
    }

    public override BinaryOperator VisitLessOperation(ASPParser.LessOperationContext context)
    {
        return new LessThan();
    }

    public override BinaryOperator VisitGreaterOperation(ASPParser.GreaterOperationContext context)
    {
        return new GreaterThan();
    }

    public override BinaryOperator VisitLessOrEqOperation(ASPParser.LessOrEqOperationContext context)
    {
        return new LessOrEqualThan();
    }

    public override BinaryOperator VisitGreaterOrEqOperation(ASPParser.GreaterOrEqOperationContext context)
    {
        return new GreaterOrEqualThan();
    }
}