// <copyright file="PredicateGoalBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class that represents a builder for a predicate goal.
/// </summary>
public class PredicateGoalBuilder : IGoalBuilder
{
    private readonly PredicateGoalStateUpdater stateUpdater;
    private readonly CoinductiveChecker checker;
    private readonly DatabaseUnifier unifier;
    private readonly GoalSolver solver;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PredicateGoalBuilder"/> class.
    /// </summary>
    /// <param name="checker">The checker that the goal will use.</param>
    /// <param name="dbUnifier">A database unifier for unifying with clauses.</param>
    /// <param name="solver">A solver for solving subgoals.</param>
    /// <param name="updater">An updater for updating state after unification and subgoal solving.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="checker"/> is null,
    /// ..<paramref name="dbUnifier"/> is null,
    /// ..<paramref name="solver"/> is null,
    /// ..<paramref name="updater"/> is null,
    /// ..<paramref name="logger"/> is null.</exception>
    public PredicateGoalBuilder(
        CoinductiveChecker checker,
        DatabaseUnifier dbUnifier,
        GoalSolver solver,
        PredicateGoalStateUpdater updater,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(checker);
        ArgumentNullException.ThrowIfNull(dbUnifier);
        ArgumentNullException.ThrowIfNull(solver);
        ArgumentNullException.ThrowIfNull (updater);
        ArgumentNullException.ThrowIfNull(logger);

        this.checker = checker;
        this.unifier = dbUnifier;
        this.solver = solver;
        this.stateUpdater = updater;
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

        return new PredicateGoal(
            this.checker,
            this.unifier,
            this.solver,
            goalTerm,
            state,
            this.stateUpdater,
            this.logger);
    }
}