// <copyright file="GoalConverter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Text;

/// <summary>
/// A class for converting a goal to a structure.
/// </summary>
public class GoalConverter : TypeBaseVisitor<Structure>
{
    private readonly FunctorTableRecord functors;

    private readonly TermConverter termConverter;

    private readonly OperatorConverter operatorConverter;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoalConverter"/> class.
    /// </summary>
    /// <param name="functorTable">The table of functorTable.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="functorTable"/> is null.</exception>
    public GoalConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        this.functors = functorTable;
        this.termConverter = new TermConverter(functorTable);
        this.operatorConverter = new OperatorConverter(functorTable);
    }

    /// <summary>
    /// Converts a goal to structure, or none if conversion has failed.
    /// </summary>
    /// <param name="goal">The goal to convert.</param>
    /// <returns>The converted goal, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if goal is null.</exception>
    public IOption<Structure> Convert(Goal goal)
    {
        ArgumentNullException.ThrowIfNull(goal);

        return goal.Accept(this);
    }

    /// <summary>
    /// Visits a goal to convert it.
    /// </summary>
    /// <param name="goal">The goal to convert.</param>
    /// <returns>The converted goal, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if goal is null.</exception>
    public override IOption<Structure> Visit(Forall goal)
    {
        ArgumentNullException.ThrowIfNull(goal);

        var leftMaybe = goal.VariableTerm.Accept(this.termConverter);
        if (!leftMaybe.HasValue)
        {
            return new None<Structure>();
        }

        var rightMaybe = goal.Goal.Accept(this);
        if (!rightMaybe.HasValue)
        {
            return new None<Structure>();
        }

        var convertedStruct = new Structure(this.functors.Forall,[leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);

        return new Some<Structure>(convertedStruct);
    }

    /// <summary>
    /// Visits a goal to convert it.
    /// </summary>
    /// <param name="goal">The goal to convert.</param>
    /// <returns>The converted goal, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if goal is null.</exception>
    public override IOption<Structure> Visit(Literal goal)
    {
        ArgumentNullException.ThrowIfNull(goal);

        var children = goal.Terms.Select(this.termConverter.Convert).ToArray();

        var convertedTerm = new Structure(goal.Identifier.GetCopy(), children);

        if (goal.HasStrongNegation)
        {
            convertedTerm = new Structure(this.functors.ClassicalNegation,[convertedTerm]);
        }

        if (goal.HasNafNegation)
        {
            convertedTerm = new Structure(this.functors.NegationAsFailure,[convertedTerm]);
        }

        return new Some<Structure>(convertedTerm);
    }

    /// <summary>
    /// Visits a goal to convert it.
    /// </summary>
    /// <param name="goal">The goal to convert.</param>
    /// <returns>The converted goal, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if goal is null.</exception>
    public override IOption<Structure> Visit(BinaryOperation goal)
    {
        ArgumentNullException.ThrowIfNull(goal);

        var leftMaybe = goal.Left.Accept(this.termConverter);

        var rightMaybe = goal.Right.Accept(this.termConverter);

        var functor = this.operatorConverter.Convert(goal.BinaryOperator);

        var convertedStructure = new Structure(functor,[leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);

        return new Some<Structure>(convertedStructure);
    }
}