//-----------------------------------------------------------------------
// <copyright file="Plus.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.ArithmeticOperations;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class Plus : ArithmeticOperation
{
    /// <inheritdoc/>
    public override int Evaluate(int l, int r)
    {
        return l + r;
    }

    /// <inheritdoc/>
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"+";
    }
}