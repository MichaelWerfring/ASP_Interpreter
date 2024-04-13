using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Visitors;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class GoalToBinaryOperationConverter : TypeBaseVisitor<BinaryOperation>
{
    public override IOption<BinaryOperation> Visit(BinaryOperation binaryOperation)
    {
        ArgumentNullException.ThrowIfNull(binaryOperation);
        return new Some<BinaryOperation>(binaryOperation);
    }
}