﻿// <copyright file="ICallstackCheckingResult.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;

/// <summary>
/// An interface that represents the result of a callstack check.
/// </summary>
public interface ICallstackCheckingResult
{
    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    public void Accept(ICallstackCheckingResultVisitor visitor);

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>A type of <typeparamref name="TResult"/>.</returns>
    public TResult Accept<TResult>(ICallstackCheckingResultVisitor<TResult> visitor);

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The argument.</param>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    public void Accept<TArgs>(ICallstackCheckingResultArgumentVisitor<TArgs> visitor, TArgs argument);

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The argument.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Accept<TResult, TArgs>(ICallstackCheckingResultArgumentVisitor<TResult, TArgs> visitor, TArgs argument);
}