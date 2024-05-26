// <copyright file="ArithmeticComparisonGoalBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.Comparison;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class that represents a builder for an arithmetic comparison goal.
/// </summary>
internal class ArithmeticComparisonGoalBuilder : IGoalBuilder
{
    private readonly Func<int, int, bool> predicate;
    private readonly ArithmeticEvaluator evaluator;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArithmeticComparisonGoalBuilder"/> class.
    /// </summary>
    /// <param name="predicate">The predicate for the two integer values of left and right.</param>
    /// <param name="evaluator">The arithmetic evaluator.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="predicate"/> is null,
    /// <paramref name="evaluator"/> is null,
    /// <paramref name="logger"/> is null.</exception>
    public ArithmeticComparisonGoalBuilder(Func<int, int, bool> predicate, ArithmeticEvaluator evaluator, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(logger);

        this.predicate = predicate;
        this.evaluator = evaluator;
        this.logger = logger;
    }

    /// <summary>
    /// Builds a goal based on a goal term and the solver's state.
    /// </summary>
    /// <param name="goalTerm">The term.</param>
    /// <param name="state">The current state.</param>
    /// <returns>A goal.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="goalTerm"/> is null,
    /// ..<paramref name="state"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if goal does not have two children.</exception>
    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Goal must contain a structure term with two children.", nameof(goalTerm));
        }

        return new ArithmeticComparisonGoal(
            this.evaluator,
            goalTerm.Children.ElementAt(0),
            goalTerm.Children.ElementAt(1),
            this.predicate,
            state,
            this.logger);
    }
}