using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class PredicateGoalBuilder : IGoalBuilder
{
    private readonly PredicateGoalStateUpdater _stateUpdater;
    private readonly CoinductiveChecker _checker;
    private readonly DatabaseUnifier _unifier;
    private readonly GoalSolver _solver;
    private readonly ILogger _logger;

    public PredicateGoalBuilder
    (
        CoinductiveChecker checker,
        DatabaseUnifier dbUnifier,
        GoalSolver solver,
        PredicateGoalStateUpdater updater,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(checker);
        ArgumentNullException.ThrowIfNull(dbUnifier);
        ArgumentNullException.ThrowIfNull(solver);
        ArgumentNullException.ThrowIfNull (updater);
        ArgumentNullException.ThrowIfNull(logger);

        _checker = checker;
        _unifier = dbUnifier;
        _solver = solver;
        _stateUpdater = updater;
        _logger = logger;
    }

    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        return new PredicateGoal
        (
            _checker,
            _unifier,
            _solver,
            goalTerm,
            state,
            _stateUpdater,
            _logger
       );
    }
}