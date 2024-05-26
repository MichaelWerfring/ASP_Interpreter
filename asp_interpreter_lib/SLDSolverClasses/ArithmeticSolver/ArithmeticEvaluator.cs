// <copyright file="ArithmeticEvaluator.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

/// <summary>
/// A class for evaluating arithmetic expressions.
/// </summary>
public class ArithmeticEvaluator : ISimpleTermVisitor<IOption<int>>
{
    private readonly IImmutableDictionary<string, Func<int, int, IOption<int>>> evaluationFunctions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArithmeticEvaluator"/> class.
    /// </summary>
    /// <param name="functorTable">The functor table to use for arithmetic funtors.</param>
    public ArithmeticEvaluator(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        this.evaluationFunctions = new Dictionary<string, Func<int, int, IOption<int>>>()
        {
            { functorTable.Addition, (left, right) => new Some<int>(left + right) },
            { functorTable.Subtraction, (left, right) => new Some<int>(left - right) },
            { functorTable.Multiplication, (left, right) => new Some<int>(left * right) },
            {
                functorTable.Division, (left, right) =>
                {
                    if (right != 0)
                    {
                        return new Some<int>(left / right);
                    }
                    else
                    {
                        return new None<int>();
                    }
                }
            },
            { functorTable.Power, (left, right) => new Some<int>(Convert.ToInt32(Math.Pow(left, right))) },
        }.ToImmutableDictionary();
    }

    /// <summary>
    /// Attempts to evaluate a term as an arithmetic expression.
    /// </summary>
    /// <param name="term">The term to evaluate.</param>
    /// <returns>An integer that the term evaluates to, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<int> Evaluate(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    /// <summary>
    /// Visits a term to evaluate it as an arithmetic expression.
    /// </summary>
    /// <param name="integer">The term to evaluate.</param>
    /// <returns>An integer that the term evaluates to, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<int> Visit(Integer integer)
    {
        ArgumentNullException.ThrowIfNull(integer);

        return new Some<int>(integer.Value);
    }

    /// <summary>
    /// Visits a term to evaluate it as an arithmetic expression.
    /// </summary>
    /// <param name="structure">The term to evaluate.</param>
    /// <returns>An integer that the term evaluates to, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<int> Visit(Structure structure)
    {
        ArgumentNullException.ThrowIfNull(structure);

        if (structure.Children.Count != 2)
        {
            return new None<int>();
        }

        int leftVal;
        int rightVal;
        try
        {
            leftVal = structure.Children.ElementAt(0).Accept(this).GetValueOrThrow();
            rightVal = structure.Children.ElementAt(1).Accept(this).GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }

        this.evaluationFunctions.TryGetValue(structure.Functor, out Func<int, int, IOption<int>>? func);

        if (func == null)
        {
            return new None<int>();
        }

        return func.Invoke(leftVal, rightVal);
    }

    /// <summary>
    /// Visits a term to evaluate it as an arithmetic expression.
    /// </summary>
    /// <param name="variable">The term to evaluate.</param>
    /// <returns>Always none in this case.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<int> Visit(Variable variable)
    {
        ArgumentNullException.ThrowIfNull(variable);

        return new None<int>();
    }
}