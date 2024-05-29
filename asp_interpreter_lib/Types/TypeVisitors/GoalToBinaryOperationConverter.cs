//-----------------------------------------------------------------------
// <copyright file="GoalToBinaryOperationConverter.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class GoalToBinaryOperationConverter : TypeBaseVisitor<BinaryOperation>
{
    public override IOption<BinaryOperation> Visit(BinaryOperation binOp)
    {
        ArgumentNullException.ThrowIfNull(binOp);
        return new Some<BinaryOperation>(binOp);
    }
}