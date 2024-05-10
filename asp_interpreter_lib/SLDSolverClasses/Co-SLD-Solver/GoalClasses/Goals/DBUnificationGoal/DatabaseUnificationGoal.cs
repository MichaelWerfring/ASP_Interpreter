using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using System.Collections.Immutable;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

internal class DatabaseUnificationGoal : ICoSLDGoal
{
    private readonly CoinductiveChecker _checker;

    private readonly DatabaseUnifier _databaseUnifier;

    private readonly GoalSolver _goalSolver;

    private readonly Structure _inputTarget;

    private readonly SolutionState _inputState;

    public DatabaseUnificationGoal
    (
        CoinductiveChecker checker,
        DatabaseUnifier databaseUnifier,
        GoalSolver solver,
        Structure target,
        SolutionState solutionState
    )
    {
        ArgumentNullException.ThrowIfNull(checker, nameof(checker));
        ArgumentNullException.ThrowIfNull(databaseUnifier, nameof(databaseUnifier));
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));
        
        _checker = checker;
        _databaseUnifier = databaseUnifier;
        _goalSolver = solver;
        _inputTarget = target;
        _inputState = solutionState;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        // for each way the input can "survive" the coinductive check..
        foreach (CoinductiveCheckingResult checkingResult in _checker.Check(_inputTarget, _inputState))
        {
            foreach (var solution in ResolveCheckingResults(checkingResult))
            {
                yield return solution;
            }
        }
    }

    private IEnumerable<GoalSolution> ResolveCheckingResults(CoinductiveCheckingResult checkingResult)
    {
        if (checkingResult.SuccessType == SuccessType.DeterministicSuccess)
        {
            yield return new GoalSolution
            (_inputState.Set, _inputState.Mapping, _inputState.NextInternalVariableIndex);
            yield break;
        }

        if (checkingResult.SuccessType == SuccessType.NonDeterministicSuccess)
        {
            yield return new GoalSolution
            (_inputState.Set, checkingResult.ConstrainedMapping, _inputState.NextInternalVariableIndex);
        }

        IEnumerable<DBUnificationResult> dbunifications = _databaseUnifier.GetDatabaseUnificationResults
        (checkingResult.ConstrainedTarget, checkingResult.ConstrainedMapping, _inputState.NextInternalVariableIndex);

        // for each way the target unifies with clauses in the database..
        foreach (DBUnificationResult dbunification in dbunifications)
        {
            // ..enumerate the subgoal solutions.
            foreach (var subgoalSolution in ResolveDatabaseUnifications(dbunification, checkingResult.ConstrainedTarget))
            {
                yield return subgoalSolution;
            }
        }
    }

    private IEnumerable<GoalSolution> ResolveDatabaseUnifications(DBUnificationResult result, ISimpleTerm constrainedTarget)
    {
        var newStateForSolvingSubgoals = new CoSldSolverState
        (
            result.Subgoals,
            new SolutionState
            (
                new CallStack(_inputState.Stack.TermStack.Push(constrainedTarget)),
                _inputState.Set,
                result.VariableMapping,
                result.NextInternalIndex
            )
        );

        var subgoalSolutions = _goalSolver.SolveGoals(newStateForSolvingSubgoals);

        // enumerate each way the subgoals can be satisfied
        foreach (var subgoalSolution in subgoalSolutions)
        {
            var removedSet = subgoalSolution.ResultSet.Entries.Remove(new CHSEntry(constrainedTarget, false));

            var newCHS = new CoinductiveHypothesisSet
            (
                removedSet.Add(new CHSEntry(constrainedTarget, true))
                .Select(entry => new CHSEntry(subgoalSolution.ResultMapping.ApplySubstitution(entry.Term), entry.HasSucceded))
                .ToImmutableSortedSet(new CHSEntryComparer())
            );

            yield return new GoalSolution
            (
                newCHS,
                subgoalSolution.ResultMapping,
                subgoalSolution.NextInternalVariable
            );
        }
    }
}

