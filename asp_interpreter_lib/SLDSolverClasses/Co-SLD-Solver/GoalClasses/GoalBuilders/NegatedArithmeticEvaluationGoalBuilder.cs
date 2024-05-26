// <copyright file="NegatedArithmeticEvaluationGoalBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Unification.Constructive.Disunification;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class that represents a builder for a negated arithmetic evaluation goal.
/// </summary>
internal class NegatedArithmeticEvaluationGoalBuilder : IGoalBuilder
{
    private readonly ArithmeticEvaluator evaluator;
    private readonly IConstructiveDisunificationAlgorithm algorithm;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NegatedArithmeticEvaluationGoalBuilder"/> class.
    /// </summary>
    /// <param name="evaluator">The evaluator that the goals will use.</param>
    /// <param name="algorithm">The algorithm that the goals will use.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="evaluator"/>is null,
    /// <paramref name="algorithm"/> is null,
    /// <paramref name="logger"/> is null.</exception>
    public NegatedArithmeticEvaluationGoalBuilder(
        ArithmeticEvaluator evaluator,
        IConstructiveDisunificationAlgorithm algorithm,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        this.evaluator = evaluator;
        this.algorithm = algorithm;
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
    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(goalTerm));
        }

        return new NegatedArithmeticEvaluationGoal(
            this.evaluator,
            goalTerm.Children.ElementAt(0),
            goalTerm.Children.ElementAt(1),
            state,
            this.algorithm,
            this.logger);
    }
}