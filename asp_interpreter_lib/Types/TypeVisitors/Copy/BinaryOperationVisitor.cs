//-----------------------------------------------------------------------
// <copyright file="BinaryOperationVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors.Copy;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;

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