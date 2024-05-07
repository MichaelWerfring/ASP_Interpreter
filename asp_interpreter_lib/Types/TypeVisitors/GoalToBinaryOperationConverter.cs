using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Visitors;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class GoalToBinaryOperationConverter : TypeBaseVisitor<BinaryOperation>
{
    public override IOption<BinaryOperation> Visit(BinaryOperation binOp)
    {
        ArgumentNullException.ThrowIfNull(binOp);
        return new Some<BinaryOperation>(binOp);
    }
}