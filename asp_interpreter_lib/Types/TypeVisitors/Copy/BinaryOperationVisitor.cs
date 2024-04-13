using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.TypeVisitors.Copy;

public class BinaryOperationVisitor(TypeBaseVisitor<ITerm> termCopyVisitor) : TypeBaseVisitor<BinaryOperation>
{
    public override IOption<BinaryOperation> Visit(BinaryOperation operation)
    {
        var leftCopy = operation.Left.Accept(termCopyVisitor).GetValueOrThrow(
            "The given left term cannot be read!");
        var rightCopy = operation.Right.Accept(termCopyVisitor).GetValueOrThrow(
            "The given right term cannot be read!");

        return new Some<BinaryOperation>(new BinaryOperation(leftCopy, operation.BinaryOperator, rightCopy));
    }
}