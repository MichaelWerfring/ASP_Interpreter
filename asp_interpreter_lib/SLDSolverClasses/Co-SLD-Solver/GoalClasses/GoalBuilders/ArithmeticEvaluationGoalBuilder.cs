// <copyright file="ArithmeticEvaluationGoalBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Unification.Constructive.Unification;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class that represents a builder for an arithmetic evaluation goal.
/// </summary>
public class ArithmeticEvaluationGoalBuilder : IGoalBuilder
{
    private readonly ArithmeticEvaluator evaluator;
    private readonly IConstructiveUnificationAlgorithm algorithm;
    private readonly ILogger logger;
    private readonly SolverStateUpdater updater;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArithmeticEvaluationGoalBuilder"/> class.
    /// </summary>
    /// <param name="updater">The solver state updater for the goals.</param>
    /// <param name="evaluator">The evaluator for the goals.</param>
    /// <param name="algorithm">The algorithm for the goals.</param>
    /// <param name="logger">The logger for the goals.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="updater"/> is null,
    /// <paramref name="evaluator"/> is null,
    /// <paramref name="algorithm"/> is null,
    /// <paramref name="logger"/> is null.</exception>
    public ArithmeticEvaluationGoalBuilder(
        SolverStateUpdater updater,
        ArithmeticEvaluator evaluator,
        IConstructiveUnificationAlgorithm algorithm,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(updater);
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        this.evaluator = evaluator;
        this.algorithm = algorithm;
        this.logger = logger;
        this.updater = updater;
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
            throw new ArgumentException("Goal must be a structure term with two children.", nameof(goalTerm));
        }

        return new ArithmeticEvaluationGoal(
            this.updater,
            this.evaluator,
            goalTerm.Children.ElementAt(0),
            goalTerm.Children.ElementAt(1),
            state,
            this.algorithm,
            this.logger);
    }
}