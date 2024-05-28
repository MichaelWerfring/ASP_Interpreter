﻿//-----------------------------------------------------------------------
// <copyright file="BinaryOperator.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// Represents generic a binary operator.
/// </summary>
public abstract class BinaryOperator : IVisitableType
{
    /// <summary>
    /// Accepts a <see cref="TypeBaseVisitor{T}"/> and returns the result of the given operation.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="visitor">The visitor to accept.</param>
    /// <returns>Either none if the visitor fails to execute the corresponding
    /// method or the result wrapped into an instance of <see cref="Some{T}"/>class.</returns>
    /// <exception cref="ArgumentNullException">If the visitor is null.</exception>
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}