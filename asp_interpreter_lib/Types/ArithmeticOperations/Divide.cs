//-----------------------------------------------------------------------
// <copyright file="Divide.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.ArithmeticOperations;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// Represents the division operation.
/// </summary>
public class Divide : ArithmeticOperation
{
    /// <summary>
    /// Evaluates the given operation.
    /// </summary>
    /// <param name="l">The right hand side of the operation.</param>
    /// <param name="r">The left hand side of the operation.</param>
    /// <returns>The result of the operation.</returns>
    /// <exception cref="DivideByZeroException">Is thrown if the right hand side is 0.</exception>
    public override int Evaluate(int l, int r)
    {
        if (r == 0)
        {
            throw new DivideByZeroException();
        }

        return l / r;
    }

    /// <summary>
    /// Accepts a <see cref="TypeBaseVisitor{T}"/> and returns the result of the given operation.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="visitor">The visitor to accept.</param>
    /// <returns>Either none if the visitor fails to execute the corresponding
    /// method or the result wrapped into an instance of <see cref="Some{T}"/>class.</returns>
    /// <exception cref="ArgumentNullException">If the visitor is null.</exception>
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    /// <summary>
    /// Returns the string representation of the type.
    /// </summary>
    /// <returns>The string representation of the type.</returns>
    public override string ToString()
    {
        return $"/";
    }
}