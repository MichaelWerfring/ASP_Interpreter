﻿// <copyright file="ArithmeticEvaluationGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Unification.Constructive.Unification;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class that represents an evaluation goal, or otherwise called an "is".
/// </summary>
internal class ArithmeticEvaluationGoal : ICoSLDGoal, ISimpleTermArgsVisitor<IOption<GoalSolution>, int>
{
    private readonly SolverStateUpdater updater;
    private readonly ArithmeticEvaluator evaluator;
    private readonly ISimpleTerm left;
    private readonly ISimpleTerm right;
    private readonly SolutionState state;
    private readonly IConstructiveUnificationAlgorithm algorithm;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArithmeticEvaluationGoal"/> class.
    /// </summary>
    /// <param name="updater">A state updater to use after success.</param>
    /// <param name="evaluator">The arithmetic evaluator.</param>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <param name="state">The input solution state.</param>
    /// <param name="algorithm">The unification algorithm.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="evaluator"/> is null,
    /// ..<paramref name="left"/> is null,
    /// <paramref name="right"/> is null,
    /// <paramref name="logger"/> is null.</exception>
    public ArithmeticEvaluationGoal(
        SolverStateUpdater updater,
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        SolutionState state,
        IConstructiveUnificationAlgorithm algorithm,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(updater);
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        this.updater = updater;
        this.evaluator = evaluator;
        this.left = left;
        this.right = right;
        this.state = state;
        this.algorithm = algorithm;
        this.logger = logger;
    }

    /// <summary>
    /// Attempts to solve the goal.
    /// </summary>
    /// <returns>An enumeration of all the ways the goal can be solved.</returns>
    public IEnumerable<GoalSolution> TrySatisfy()
    {
        this.logger.LogInfo($"Attempting to solve arithmetic evaluation goal: {this.left}, {this.right}");
        this.logger.LogTrace($"Input state is: {this.state}");

        IOption<int> rightEvalMaybe = this.evaluator.Evaluate(this.right);

        int rightEval;
        try
        {
            rightEval = rightEvalMaybe.GetValueOrThrow();
        }
        catch
        {
            yield break;
        }

        var solutionMaybe = this.left.Accept(this, rightEval);
        if (!solutionMaybe.HasValue)
        {
            yield break;
        }

        this.logger.LogInfo($"Solved arithmetic evaluation goal: {this.left}, {this.right}");

        yield return solutionMaybe.GetValueOrThrow();
    }

    /// <summary>
    /// Visits a case where left is variable.
    /// </summary>
    /// <param name="var">The variable.</param>
    /// <param name="integer">The right evaluation.</param>
    /// <returns>A goal solution, or none in case of failure.</returns>
    /// <exception cref="ArgumentException">Thrown if constructive target building fails.</exception>
    /// <exception cref="ArgumentNullException">Thrown if var is null.</exception>
    public IOption<GoalSolution> Visit(Variable var, int integer)
    {
        ArgumentNullException.ThrowIfNull(var);

        var targetEither = ConstructiveTargetBuilder.Build(var, new Integer(integer), this.state.Mapping);
        if (!targetEither.IsRight)
        {
            this.logger.LogError(targetEither.GetLeftOrThrow().Message);
            throw new ArgumentException(nameof(this.state));
        }

        var target = targetEither.GetRightOrThrow();

        IOption<VariableMapping> unificationResult = this.algorithm.Unify(target);
        if (!unificationResult.HasValue)
        {
            return new None<GoalSolution>();
        }

        VariableMapping unification = unificationResult.GetValueOrThrow();
        this.logger.LogTrace($"Unifying mapping is {unification}");

        var updatedMapping = this.state.Mapping.Update(unification).GetValueOrThrow();
        this.logger.LogTrace($"Updated mapping is {updatedMapping}");

        var flattenedMapping = updatedMapping.Flatten();
        this.logger.LogTrace($"Flattened mapping is {flattenedMapping}");

        CoinductiveHypothesisSet updatedCHS = this.updater.UpdateCHS(this.state.CHS, flattenedMapping);
        this.logger.LogTrace($"Updated CHS is {updatedCHS}");

        CallStack updatedStack = this.updater.UpdateCallstack(this.state.Callstack, flattenedMapping);
        this.logger.LogTrace($"Updated callstack is {updatedStack}");

        return new Some<GoalSolution>(
            new GoalSolution(
                updatedCHS,
                flattenedMapping,
                updatedStack,
                this.state.NextInternalVariableIndex));
    }

    /// <summary>
    /// Visits a case where left is structure.
    /// </summary>
    /// <param name="structure">The structure.</param>
    /// <param name="integer">The right evaluation.</param>
    /// <returns>Always none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if structure is null.</exception>
    public IOption<GoalSolution> Visit(Structure structure, int integer)
    {
        ArgumentNullException.ThrowIfNull(structure);

        return new None<GoalSolution>();
    }

    /// <summary>
    /// Visits a case where left is integer.
    /// </summary>
    /// <param name="integer">The structure.</param>
    /// <param name="rightEval">The right evaluation.</param>
    /// <returns>A goalsolution, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if integer is null.</exception>
    public IOption<GoalSolution> Visit(Integer integer, int rightEval)
    {
        ArgumentNullException.ThrowIfNull(integer);

        if (integer.Value != rightEval)
        {
            return new None<GoalSolution>();
        }

        return new Some<GoalSolution>(
            new GoalSolution(
                this.state.CHS,
                this.state.Mapping,
                this.state.Callstack,
                this.state.NextInternalVariableIndex));
    }
}