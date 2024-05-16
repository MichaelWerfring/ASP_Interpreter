using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class BinaryOperatorVisitor(ILogger logger) : ASPParserBaseVisitor<IOption<BinaryOperator>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<BinaryOperator> VisitEqualityOperation(ASPParser.EqualityOperationContext context)
    {
        return new Some<BinaryOperator>(new Equality());
    }

    public override IOption<BinaryOperator> VisitDisunificationOperation(ASPParser.DisunificationOperationContext context)
    {
        return new Some<BinaryOperator>(new Disunification());
    }

    public override IOption<BinaryOperator> VisitLessOperation(ASPParser.LessOperationContext context)
    {
        return new Some<BinaryOperator>(new LessThan());
    }

    public override IOption<BinaryOperator> VisitGreaterOperation(ASPParser.GreaterOperationContext context)
    {
        return new Some<BinaryOperator>(new GreaterThan());
    }

    public override IOption<BinaryOperator> VisitLessOrEqOperation(ASPParser.LessOrEqOperationContext context)
    {
        return new Some<BinaryOperator>(new LessOrEqualThan());
    }

    public override IOption<BinaryOperator> VisitGreaterOrEqOperation(ASPParser.GreaterOrEqOperationContext context)
    {
        return new Some<BinaryOperator>(new GreaterOrEqualThan());
    }
    
    public override IOption<BinaryOperator> VisitIsOperation(ASPParser.IsOperationContext context)
    {
        return new Some<BinaryOperator>(new Is());
    }
}