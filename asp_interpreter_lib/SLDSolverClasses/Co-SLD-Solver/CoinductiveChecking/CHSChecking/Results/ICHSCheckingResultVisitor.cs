// <copyright file="ICHSCheckingResultVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;

/// <summary>
/// An interface for a chs result visitor.
/// </summary>
public interface ICHSCheckingResultVisitor
{
    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    public void Visit(CHSDeterministicFailureResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    public void Visit(CHSDeterministicSuccessResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    public void Visit(CHSNoMatchOrConstrainmentResult result);
}

/// <summary>
/// An interface for a chs result visitor that returns a value.
/// </summary>
/// <typeparam name="T">The return type.</typeparam>
public interface ICHSCheckingResultVisitor<T>
{
    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(CHSDeterministicFailureResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(CHSDeterministicSuccessResult result);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(CHSNoMatchOrConstrainmentResult result);
}

/// <summary>
/// An interface for a chs result visitor that has an additional argument.
/// </summary>
/// <typeparam name="TArgs">The argument type.</typeparam>
public interface ICHSCheckingResultArgumentsVisitor<TArgs>
{
    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    public void Visit(CHSDeterministicFailureResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    public void Visit(CHSDeterministicSuccessResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    public void Visit(CHSNoMatchOrConstrainmentResult result, TArgs argument);
}

/// <summary>
/// An interface for a chs result visitor that has an additional argument and returns a value.
/// </summary>
/// <typeparam name="TResult">The result type.</typeparam>
/// <typeparam name="TArgs">The argument type.</typeparam>
public interface ICHSCheckingResultArgumentsVisitor<TResult, TArgs>
{
    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Visit(CHSDeterministicFailureResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Visit(CHSDeterministicSuccessResult result, TArgs argument);

    /// <summary>
    /// Visits a result.
    /// </summary>
    /// <param name="result">The result to visit.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Visit(CHSNoMatchOrConstrainmentResult result, TArgs argument);
}