using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class ArithmeticOperationVisitor(ILogger logger) : ASPBaseVisitor<IOption<ArithmeticOperation>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<ArithmeticOperation> VisitPlusOperation(ASPParser.PlusOperationContext context)
    {
        return new Some<ArithmeticOperation>(new Plus());
    }

    public override IOption<ArithmeticOperation> VisitMinusOperation(ASPParser.MinusOperationContext context)
    {
        return new Some<ArithmeticOperation>(new Minus());
    }

    public override IOption<ArithmeticOperation> VisitTimesOperation(ASPParser.TimesOperationContext context)
    {
        return new Some<ArithmeticOperation>(new Multiply());
    }

    public override IOption<ArithmeticOperation> VisitDivOperation(ASPParser.DivOperationContext context)
    {
        return new Some<ArithmeticOperation>(new Divide());
    }
    
    public override IOption<ArithmeticOperation> VisitPowerOperation(ASPParser.PowerOperationContext context)
    {
        return new Some<ArithmeticOperation>(new Power());
    }
}