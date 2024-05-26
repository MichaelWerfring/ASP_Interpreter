// <copyright file="ArithmeticComparisonGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.Comparison;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

/// <summary>
/// Represents an arithmetic comparison goal.
/// </summary>
public class ArithmeticComparisonGoal : ICoSLDGoal
{
    private readonly ArithmeticEvaluator evaluator;
    private readonly ISimpleTerm left;
    private readonly ISimpleTerm right;
    private readonly Func<int, int, bool> predicate;
    private readonly SolutionState inputstate;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArithmeticComparisonGoal"/> class.
    /// </summary>
    /// <param name="evaluator">The arithmetic evaluator.</param>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <param name="predicate">A predicate to determine the truth value of the evaluation.</param>
    /// <param name="solutionState">The input solution state.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="evaluator"/> is null,
    /// ..<paramref name="left"/> is null,
    /// <paramref name="right"/> is null,
    /// <paramref name="predicate"/> is null,
    /// <paramref name="solutionState"/> is null,
    /// <paramref name="logger"/> is null.</exception>
    public ArithmeticComparisonGoal(
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        Func<int, int, bool> predicate,
        SolutionState solutionState,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(solutionState);
        ArgumentNullException.ThrowIfNull(logger);

        this.evaluator = evaluator;
        this.left = left;
        this.right = right;
        this.predicate = predicate;
        this.inputstate = solutionState;
        this.logger = logger;
    }

    /// <summary>
    /// Attempts to solve the goal.
    /// </summary>
    /// <returns>An enumeration of all the ways the goal can be solved.</returns>
    public IEnumerable<GoalSolution> TrySatisfy()
    {
        this.logger.LogInfo($"Attempting to solve arithmetic comparison goal: {this.left}, {this.right}");
        this.logger.LogTrace($"Input state is: {this.inputstate}");

        var leftIntegerMaybe = TermFuncs.ReturnIntegerOrNone(this.left);
        if (!leftIntegerMaybe.HasValue)
        {
            yield break;
        }

        var leftInteger = leftIntegerMaybe.GetValueOrThrow();

        var rightEvaluationMaybe = this.evaluator.Evaluate(this.right);

        if (!rightEvaluationMaybe.HasValue)
        {
            yield break;
        }

        if (!this.predicate(leftInteger.Value, rightEvaluationMaybe.GetValueOrThrow()))
        {
            yield break;
        }

        this.logger.LogInfo($"Solved arithmetic comparison goal: {this.left}, {this.right}");

        yield return new GoalSolution(
            this.inputstate.CHS,
            this.inputstate.Mapping,
            this.inputstate.Callstack,
            this.inputstate.NextInternalVariableIndex);
    }
}