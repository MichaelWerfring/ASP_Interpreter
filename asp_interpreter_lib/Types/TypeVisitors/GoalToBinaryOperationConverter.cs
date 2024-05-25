using Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Visitors;

namespace Asp_interpreter_lib.Types.TypeVisitors;

public class GoalToBinaryOperationConverter : TypeBaseVisitor<BinaryOperation>
{
    public override IOption<BinaryOperation> Visit(BinaryOperation binOp)
    {
        ArgumentNullException.ThrowIfNull(binOp);
        return new Some<BinaryOperation>(binOp);
    }
}