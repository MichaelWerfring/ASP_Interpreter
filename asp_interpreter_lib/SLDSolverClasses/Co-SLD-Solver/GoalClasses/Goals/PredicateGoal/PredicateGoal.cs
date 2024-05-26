// <copyright file="PredicateGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Util.ErrorHandling;

internal class PredicateGoal : ICoSLDGoal
{
    private readonly CoinductiveChecker checker;
    private readonly DatabaseUnifier databaseUnifier;
    private readonly GoalSolver goalSolver;
    private readonly Structure inputTarget;
    private readonly SolutionState inputState;
    private readonly PredicateGoalStateUpdater stateUpdater;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PredicateGoal"/> class.
    /// </summary>
    /// <param name="checker">A coinductive checker.</param>
    /// <param name="databaseUnifier">A unifier for unifying with clauses in the database.</param>
    /// <param name="solver">A solver for solving subgoals.</param>
    /// <param name="target">The predicate to solve.</param>
    /// <param name="solutionState">The input solution state.</param>
    /// <param name="stateUpdater">An updater for updating after unification and subgoal solving.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="checker"/> is null,
    /// <paramref name="databaseUnifier"/> is null,
    /// <paramref name="solver"/> is null,
    /// <paramref name="target"/> is null,
    /// <paramref name="solutionState"/> is null,
    /// <paramref name="logger"/> is null,
    /// <paramref name="stateUpdater"/> is null.</exception>
    public PredicateGoal(
        CoinductiveChecker checker,
        DatabaseUnifier databaseUnifier,
        GoalSolver solver,
        Structure target,
        SolutionState solutionState,
        PredicateGoalStateUpdater stateUpdater,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(checker, nameof(checker));
        ArgumentNullException.ThrowIfNull(databaseUnifier, nameof(databaseUnifier));
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(stateUpdater, nameof(stateUpdater));

        this.checker = checker;
        this.databaseUnifier = databaseUnifier;
        this.goalSolver = solver;
        this.inputTarget = target;
        this.inputState = solutionState;
        this.stateUpdater = stateUpdater;
        this.logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        this.logger.LogInfo($"Attempting to solve predicate goal {this.inputTarget}");
        this.logger.LogTrace($"Input state is: {this.inputState}");

        // for each way the input can "survive" the coinductive check..
        foreach (CoinductiveCheckingResult checkingResult in this.checker.Check(this.inputTarget, this.inputState))
        {
            // enumerate the ways they can be solved.
            foreach (var solution in this.SolveCoinductiveCheckingResults(checkingResult))
            {
                yield return solution;
            }
        }
    }

    private IEnumerable<GoalSolution> SolveCoinductiveCheckingResults(CoinductiveCheckingResult checkingResult)
    {
        if (checkingResult.SuccessType == SuccessType.DeterministicSuccess)
        {
            yield return this.stateUpdater.ConstructCoinductiveSuccessSolution(this.inputState, checkingResult.Mapping);
            yield break;
        }

        if (checkingResult.SuccessType == SuccessType.NonDeterministicSuccess)
        {
            yield return this.stateUpdater.ConstructCoinductiveSuccessSolution(this.inputState, checkingResult.Mapping);
        }

        IEnumerable<DBUnificationResult> dbunifications = this.databaseUnifier.GetDatabaseUnificationResults(
            checkingResult.Target,
            checkingResult.Mapping,
            this.inputState.NextInternalVariableIndex);

        // for each way the constrainedTarget unifies with clauses in the database..
        foreach (DBUnificationResult dbunification in dbunifications)
        {
            // ..enumerate the subgoal solutions.
            foreach (var subgoalSolution in this.SolveDatabaseUnificationResults(dbunification))
            {
                yield return subgoalSolution;
            }
        }
    }

    private IEnumerable<GoalSolution> SolveDatabaseUnificationResults(DBUnificationResult result)
    {
        CoSldSolverState nextState = this.stateUpdater.BuildStateForSolvingBodyGoals(
            this.inputState.CHS,
            this.inputState.Callstack,
            result.NewMapping,
            result.RenamedClause,
            this.inputTarget,
            result.NextInternalIndex);

        // enumerate each way the subgoal can be satisfied. Update and yield return.
        foreach (GoalSolution subgoalSolution in this.goalSolver.SolveGoals(nextState))
        {
            GoalSolution updatedSolution = this.stateUpdater.UpdateGoalSolution(subgoalSolution, this.inputTarget);

            yield return updatedSolution;
        }
    }
}