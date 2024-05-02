using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class BinaryOperatorNegator : TypeBaseVisitor<BinaryOperator>
{
    public override IOption<BinaryOperator> Visit(Disunification _)
    {
        return new Some<BinaryOperator>(new Equality());
    }

    public override IOption<BinaryOperator> Visit(Equality _)
    {
        return new Some<BinaryOperator>(new Disunification());
    }

    public override IOption<BinaryOperator> Visit(GreaterOrEqualThan _)
    {
        return new Some<BinaryOperator>(new LessThan());
    }

    public override IOption<BinaryOperator> Visit(GreaterThan _)
    {
        return new Some<BinaryOperator>(new LessOrEqualThan());
    }

    public override IOption<BinaryOperator> Visit(LessOrEqualThan _)
    {
        return new Some<BinaryOperator>(new GreaterThan());
    }

    public override IOption<BinaryOperator> Visit(LessThan _)
    {
        return new Some<BinaryOperator>(new GreaterOrEqualThan());
    }
}