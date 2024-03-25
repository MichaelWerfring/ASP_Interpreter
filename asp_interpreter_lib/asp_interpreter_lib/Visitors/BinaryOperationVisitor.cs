using asp_interpreter_lib.Types.BinaryOperations;

namespace asp_interpreter_lib.Visitors;

public class BinaryOperationVisitor : ASPBaseVisitor<BinaryOperation>
{
    public override BinaryOperation VisitEqualityOperation(ASPParser.EqualityOperationContext context)
    {
        return base.VisitEqualityOperation(context);
    }

    public override BinaryOperation VisitDisunificationOperation(ASPParser.DisunificationOperationContext context)
    {
        return base.VisitDisunificationOperation(context);
    }

    public override BinaryOperation VisitLessOperation(ASPParser.LessOperationContext context)
    {
        return base.VisitLessOperation(context);
    }

    public override BinaryOperation VisitGreaterOperation(ASPParser.GreaterOperationContext context)
    {
        return base.VisitGreaterOperation(context);
    }

    public override BinaryOperation VisitLessOrEqOperation(ASPParser.LessOrEqOperationContext context)
    {
        return base.VisitLessOrEqOperation(context);
    }

    public override BinaryOperation VisitGreaterOrEqOperation(ASPParser.GreaterOrEqOperationContext context)
    {
        return base.VisitGreaterOrEqOperation(context);
    }
}