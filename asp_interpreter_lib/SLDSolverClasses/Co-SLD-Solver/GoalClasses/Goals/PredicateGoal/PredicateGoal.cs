// <copyright file="PredicateGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using Asp_interpreter_lib.Util.ErrorHandling;

internal class PredicateGoal : ICoSLDGoal
{
    private readonly CoinductiveChecker _checker;
    private readonly DatabaseUnifier _databaseUnifier;
    private readonly GoalSolver _goalSolver;
    private readonly Structure _inputTarget;
    private readonly SolutionState _inputState;
    private readonly PredicateGoalStateUpdater _stateUpdater;
    private readonly ILogger _logger;

    public PredicateGoal
    (
        CoinductiveChecker checker,
        DatabaseUnifier databaseUnifier,
        GoalSolver solver,
        Structure target,
        SolutionState solutionState,
        PredicateGoalStateUpdater stateUpdater,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(checker, nameof(checker));
        ArgumentNullException.ThrowIfNull(databaseUnifier, nameof(databaseUnifier));
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(stateUpdater, nameof(stateUpdater));

        _checker = checker;
        _databaseUnifier = databaseUnifier;
        _goalSolver = solver;
        _inputTarget = target;
        _inputState = solutionState;
        _stateUpdater = stateUpdater;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve predicate goal {_inputTarget}");
        _logger.LogTrace($"Input state is: {_inputState}");

        // for each way the input can "survive" the coinductive check..
        foreach (CoinductiveCheckingResult checkingResult in _checker.Check(_inputTarget, _inputState))
        {
            // enumerate the ways they can be solved.
            foreach (var solution in SolveCoinductiveCheckingResults(checkingResult))
            {
                yield return solution;
            }
        }
    }

    private IEnumerable<GoalSolution> SolveCoinductiveCheckingResults(CoinductiveCheckingResult checkingResult)
    {
        if (checkingResult.SuccessType == SuccessType.DeterministicSuccess)
        {
            yield return _stateUpdater.ConstructCoinductiveSuccessSolution(_inputState, checkingResult.Mapping);
            yield break;
        }

        if (checkingResult.SuccessType == SuccessType.NonDeterministicSuccess)
        {
            yield return _stateUpdater.ConstructCoinductiveSuccessSolution(_inputState, checkingResult.Mapping);
        }

        IEnumerable<DBUnificationResult> dbunifications = _databaseUnifier.GetDatabaseUnificationResults
        (
            checkingResult.Target,
            checkingResult.Mapping,
            _inputState.NextInternalVariableIndex
        );

        // for each way the constrainedTarget unifies with clauses in the database..
        foreach (DBUnificationResult dbunification in dbunifications)
        {
            // ..enumerate the subgoal solutions.
            foreach (var subgoalSolution in SolveDatabaseUnificationResults(dbunification))
            {
                yield return subgoalSolution;
            }
        }
    }

    private IEnumerable<GoalSolution> SolveDatabaseUnificationResults(DBUnificationResult result)
    {
        CoSldSolverState nextState = _stateUpdater.BuildStateForSolvingBodyGoals
        (
            _inputState.CHS,
            _inputState.Callstack, 
            result.NewMapping,
            result.RenamedClause,
            _inputTarget, 
            result.NextInternalIndex
        );

        // enumerate each way the subgoal can be satisfied. Update and yield return.
        foreach (GoalSolution subgoalSolution in _goalSolver.SolveGoals(nextState))
        {
            GoalSolution updatedSolution = _stateUpdater.UpdateGoalSolution(subgoalSolution, _inputTarget);

            yield return updatedSolution;
        }
    }
}