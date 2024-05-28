﻿//-----------------------------------------------------------------------
// <copyright file="Equality.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// Represents the equality operator.
/// </summary>
public class Equality : BinaryOperator, IVisitableType
{
    /// <summary>
    /// Returns the string representation of the type.
    /// </summary>
    /// <returns>The string representation of the type.</returns>
    public override string ToString()
    {
        return "=";
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
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}