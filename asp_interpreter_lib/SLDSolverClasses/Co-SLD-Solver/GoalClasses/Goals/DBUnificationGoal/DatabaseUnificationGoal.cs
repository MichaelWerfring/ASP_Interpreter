using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using System.Collections.Immutable;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

internal class DatabaseUnificationGoal : ICoSLDGoal
{
    private readonly ILogger _logger;
    private readonly CHSChecker _chsChecker;
    private readonly CallstackChecker _callstackChecker;
    private readonly DatabaseUnifier _databaseUnifier;

    private readonly GoalSolver _goalSolver;

    private readonly Structure _inputTarget;

    private readonly SolutionState _inputState;

    public DatabaseUnificationGoal
    (
        CHSChecker checker,
        CallstackChecker callstackChecker,
        IDatabase database,
        CoSLDGoalMapper mapper,
        Structure target,
        SolutionState solutionState,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(checker, nameof(checker));
        ArgumentNullException.ThrowIfNull(callstackChecker, nameof(callstackChecker));
        ArgumentNullException.ThrowIfNull (database, nameof(database));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _logger = logger;

        _chsChecker =checker;

        _callstackChecker =callstackChecker;

        _goalSolver = new GoalSolver(mapper, database, _logger);

        _databaseUnifier = new DatabaseUnifier(new StandardConstructiveUnificationAlgorithm(false), database);

        _inputTarget = target;

        _inputState = solutionState;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        ICHSCheckingResult chsCheckingResult = _chsChecker.CheckCHS(_inputTarget, _inputState);

        if (chsCheckingResult is CHSDeterministicFailureResult) {yield break;}

        if (chsCheckingResult is CHSDeterministicSuccessResult)
{
            yield return new GoalSolution(_inputState.CurrentSet, _inputState.CurrentMapping, _inputState.NextInternalVariableIndex);
        }

        var chsConstraintmentResult = (CHSConstrainmentResult) chsCheckingResult;

        foreach (var constraintment in chsConstraintmentResult.ConstrainmentResults)
        {
            Structure targetwithConstraintment = (Structure)constraintment.ApplySubstitution(_inputTarget);

            ICallstackCheckingResult callstackCheckingResult = 
                _callstackChecker.CheckCallstack(targetwithConstraintment, constraintment, _inputState.CurrentStack);

            if (callstackCheckingResult is CallstackDeterministicFailureResult)
            {
                yield break;
            }

            if (callstackCheckingResult is CallstackDeterministicSuccessResult)
            {
                yield return new 
                    GoalSolution(_inputState.CurrentSet, constraintment, _inputState.NextInternalVariableIndex);
                yield break;
            }

            if(callstackCheckingResult is CallstackNondeterministicSuccessResult)
            {
                yield return new GoalSolution(_inputState.CurrentSet, constraintment, _inputState.NextInternalVariableIndex);
            }

            IEnumerable<DBUnificationResult> dbunifications = _databaseUnifier.GetDatabaseUnificationResults
            (targetwithConstraintment, constraintment, _inputState.NextInternalVariableIndex);

            foreach (var dbunification in dbunifications)
            {
                var newStateForSolvingSubgoals = new CoSldSolverState
                (
                    dbunification.Subgoals,
                    new SolutionState
                    (
                        new CallStack(_inputState.CurrentStack.TermStack.Push(targetwithConstraintment)),
                        _inputState.CurrentSet,
                        dbunification.VariableMapping,
                        dbunification.NextInternalIndex
                    )
                );

                var subgoalSolutions = _goalSolver.SolveGoals(newStateForSolvingSubgoals);

                foreach (var subgoalSolution in subgoalSolutions)
                {
                    var newCHS = new CoinductiveHypothesisSet
                    (
                        subgoalSolution.ResultSet.Terms.Add(targetwithConstraintment)
                        .Select(subgoalSolution.ResultMapping.ApplySubstitution)
                        .ToImmutableHashSet(new SimpleTermEqualityComparer())
                    );

                    yield return new GoalSolution(newCHS, subgoalSolution.ResultMapping, subgoalSolution.NextInternalVariable);
                }
            }
        }


    }
}   

