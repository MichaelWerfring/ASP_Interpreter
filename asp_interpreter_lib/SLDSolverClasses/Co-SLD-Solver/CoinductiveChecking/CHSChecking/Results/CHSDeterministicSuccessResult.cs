// <copyright file="CHSDeterministicSuccessResult.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;

public class CHSDeterministicSuccessResult : ICHSCheckingResult
{
    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <exception cref="ArgumentNullException">Thrown if visitor is null.</exception>
    public void Accept(ICHSCheckingResultVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>A type of <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if visitor is null.</exception>
    public TResult Accept<TResult>(ICHSCheckingResultVisitor<TResult> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The argument.</param>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..visitor is null.
    /// ..argument is null.</exception>
    public void Accept<TArgs>(ICHSCheckingResultArgumentsVisitor<TArgs> visitor, TArgs argument)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(argument);

        visitor.Visit(this, argument);
    }

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The argument.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..visitor is null.
    /// ..argument is null.</exception>
    public TResult Accept<TResult, TArgs>(ICHSCheckingResultArgumentsVisitor<TResult, TArgs> visitor, TArgs argument)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(argument);

        return visitor.Visit(this, argument);
    }
}