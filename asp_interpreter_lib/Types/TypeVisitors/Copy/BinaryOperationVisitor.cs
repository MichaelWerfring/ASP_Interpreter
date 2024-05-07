using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors.Copy;

public class BinaryOperationVisitor(TypeBaseVisitor<ITerm> termCopyVisitor) : TypeBaseVisitor<BinaryOperation>
{
    public override IOption<BinaryOperation> Visit(BinaryOperation binOp)
    {
        var leftCopy = binOp.Left.Accept(termCopyVisitor).GetValueOrThrow(
            "The given left term cannot be read!");
        var rightCopy = binOp.Right.Accept(termCopyVisitor).GetValueOrThrow(
            "The given right term cannot be read!");

        return new Some<BinaryOperation>(new BinaryOperation(leftCopy, binOp.BinaryOperator, rightCopy));
    }
}