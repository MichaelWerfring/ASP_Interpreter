using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.ClauseRenamer;
using System.Collections.Immutable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

internal class DatabaseUnificationGoal : ICoSLDGoal
{
    private readonly VariableMappingConcatenator _concatenator = new VariableMappingConcatenator();
    private readonly VariableMappingSubstituter _substitutor = new VariableMappingSubstituter();
    private readonly ClauseVariableRenamer _renamer = new ClauseVariableRenamer();
    private readonly ConstructiveTargetBuilder _builder = new ConstructiveTargetBuilder();

    private readonly IConstructiveUnificationAlgorithm _algorithm;
    private readonly IDatabase _database;

    private readonly ISimpleTerm _target;
    private readonly CoSLDGoalMapper _mapper;

    private readonly IImmutableList<ISimpleTerm> _nextGoals;
    private readonly SolutionState _solutionState;

    private readonly CHSChecker _CHSChecker;
    private readonly CallstackChecker _callstackChecker;
    public DatabaseUnificationGoal
    (
        IConstructiveUnificationAlgorithm algorithm,
        IDatabase database,
        CoSLDGoalMapper mapper,
        ISimpleTerm target,
        IImmutableList<ISimpleTerm> nextGoals,
        SolutionState solutionState
    )
    {
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        ArgumentNullException.ThrowIfNull (mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(nextGoals, nameof(nextGoals));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        _algorithm = algorithm;
        _database = database;
        _target = target;
        _mapper = mapper;
        _nextGoals = nextGoals;
        _solutionState = solutionState;
        _CHSChecker = new CHSChecker(new FunctorTableRecord(), new GoalSolver(mapper,database));
        _callstackChecker = new CallstackChecker(new FunctorTableRecord());
    }

    public IEnumerable<CoSldSolverState> TrySatisfy()
    {
        // check CHS
        var chsCheckingResult = _CHSChecker.CheckCHS((Structure)_target, _solutionState);

        if (chsCheckingResult is CHSDeterministicFailureResult) { yield break; }

        if (chsCheckingResult is CHSDeterministicSuccessResult)
        {
            yield return new CoSldSolverState(_nextGoals, _solutionState);
            yield break;
        }

        var constrainmentResults = (CHSConstrainmentResult)chsCheckingResult;
        foreach (var result in constrainmentResults.ConstrainmentResults)
        {
            // check callstack
            var callstackCheckingResult = _callstackChecker.CheckCallstack((Structure)_target, result.ResultMapping, _solutionState.CurrentStack);

            if (callstackCheckingResult is CallstackDeterministicFailureResult) { yield break; }

            if (callstackCheckingResult is CallstackDeterministicSuccessResult)
            {
                yield return new CoSldSolverState(_nextGoals, _solutionState);
                yield break;
            }

            if (callstackCheckingResult is CallstackNondeterministicSuccessResult)
            {
                yield return new CoSldSolverState(_nextGoals, _solutionState);
            }

            var newCallstack = new CallStack(_solutionState.CurrentStack.TermStack.Push(_target));
            var newCHS = new CoinductiveHypothesisSet(_solutionState.CurrentSet.Terms.Add(_target));

            foreach (var potentialUnification in _database.GetMatchingClauses(_target))
            {
                // rename potential goal clauses
                var renamingResult = _renamer.RenameVariables(potentialUnification, _solutionState.NextInternalVariableIndex);

                // unify
                var constructiveTarget = _builder
                    .Build(_target, renamingResult.RenamedClause.First(), _solutionState.CurrentMapping);
                var unificationMaybe = _algorithm.Unify(constructiveTarget);
                if (!unificationMaybe.HasValue)
                {
                    continue;
                }

                // build concatenation
                var concatenationEither = _concatenator
                    .Concatenate(_solutionState.CurrentMapping, unificationMaybe.GetValueOrThrow());
                var concatenation = concatenationEither.GetRightOrThrow();

                // substitute 
                var substitutedSubgoals = renamingResult.RenamedClause.Skip(1)
                    .Select(x => _substitutor.Substitute(x, concatenation));

                // solve subgoals and yield solutions
                var goalSolver = new GoalSolver(_mapper, _database);
                var newState = new CoSldSolverState
                (
                    substitutedSubgoals.ToImmutableList(),
                    new SolutionState(newCallstack, newCHS, concatenation, renamingResult.NextInternalIndex)
                );
                foreach (var subgoalSolution in goalSolver.SolveGoals(newState))
                {
                    yield return new CoSldSolverState
                    (
                        _nextGoals
                        .Select(x => _substitutor.Substitute(x, subgoalSolution.ResultMapping))
                        .ToImmutableList(),
                        new SolutionState
                        (
                            _solutionState.CurrentStack,
                            subgoalSolution.ResultSet,
                            subgoalSolution.ResultMapping,
                            subgoalSolution.NextInternalVariable
                        )
                    );
                }
            }
        }
    }
}
