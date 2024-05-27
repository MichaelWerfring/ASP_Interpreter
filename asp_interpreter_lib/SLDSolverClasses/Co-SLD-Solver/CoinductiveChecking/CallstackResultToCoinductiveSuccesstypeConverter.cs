// <copyright file="CallstackResultToCoinductiveSuccesstypeConverter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for converting a callstack checking result to a coinductive success type.
/// </summary>
internal class CallstackResultToCoinductiveSuccesstypeConverter : ICallstackCheckingResultVisitor<IOption<SuccessType>>
{
    /// <summary>
    /// Converts a callstack checking result to a coinductive success type, or none if result is failure.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The success type, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="result"/> is null.</exception>
    public IOption<SuccessType> Convert(ICallstackCheckingResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.Accept(this);
    }

    /// <summary>
    /// Visits a result to convert it based on its type.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The success type, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="result"/> is null.</exception>
    public IOption<SuccessType> Visit(CallstackDeterministicFailureResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new None<SuccessType>();
    }

    /// <summary>
    /// Visits a result to convert it based on its type.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The success type, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="result"/> is null.</exception>
    public IOption<SuccessType> Visit(CallstackDeterministicSuccessResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new Some<SuccessType>(SuccessType.DeterministicSuccess);
    }

    /// <summary>
    /// Visits a result to convert it based on its type.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The success type, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="result"/> is null.</exception>
    public IOption<SuccessType> Visit(CallstackNondeterministicSuccessResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new Some<SuccessType>(SuccessType.NonDeterministicSuccess);
    }

    /// <summary>
    /// Visits a result to convert it based on its type.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>The success type, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="result"/> is null.</exception>
    public IOption<SuccessType> Visit(CallStackNoMatchResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new Some<SuccessType>(SuccessType.NoMatchOrConstrained);
    }
}