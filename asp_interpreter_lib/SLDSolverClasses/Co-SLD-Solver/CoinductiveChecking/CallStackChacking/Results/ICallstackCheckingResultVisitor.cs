// <copyright file="ICallstackCheckingResultVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;

/// <summary>
/// An interface for a callstack result visitor.
/// </summary>
public interface ICallstackCheckingResultVisitor
{
    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    public void Visit(CallstackDeterministicFailureResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    public void Visit(CallstackDeterministicSuccessResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    public void Visit(CallstackNondeterministicSuccessResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    public void Visit(CallStackNoMatchResult result);
}

/// <summary>
/// An interface for a callstack result visitor that returns a value.
/// </summary>
/// <typeparam name="T">The return type.</typeparam>
public interface ICallstackCheckingResultVisitor<T>
{
    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(CallstackDeterministicFailureResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(CallstackDeterministicSuccessResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(CallstackNondeterministicSuccessResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(CallStackNoMatchResult result);
}

/// <summary>
/// An interface for a callstack result visitor that has an additional argument.
/// </summary>
/// <typeparam name="TArgs">The argument type.</typeparam>
public interface ICallstackCheckingResultArgumentVisitor<TArgs>
{
    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    public void Visit(CallstackDeterministicFailureResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    public void Visit(CallstackDeterministicSuccessResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    public void Visit(CallstackNondeterministicSuccessResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    public void Visit(CallStackNoMatchResult result, TArgs argument);
}

/// <summary>
/// An interface for a callstack result visitor that has an additional argument and returns a value.
/// </summary>
/// <typeparam name="TResult">The result type.</typeparam>
/// <typeparam name="TArgs">The argument type.</typeparam>
public interface ICallstackCheckingResultArgumentVisitor<TResult, TArgs>
{
    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Visit(CallstackDeterministicFailureResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Visit(CallstackDeterministicSuccessResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Visit(CallstackNondeterministicSuccessResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Visit(CallStackNoMatchResult result, TArgs argument);
}