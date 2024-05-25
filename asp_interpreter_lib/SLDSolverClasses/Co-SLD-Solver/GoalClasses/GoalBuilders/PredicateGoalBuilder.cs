using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

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

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));

        if (!currentState.CurrentGoals.Any())
        {
            throw new ArgumentException("Must contain at least one goal.",nameof(currentState)); 
        }

        Structure goalTerm = currentState.CurrentGoals.ElementAt(0);

        return new PredicateGoal
        (
            _checker,
            _unifier,
            _solver,
            goalTerm,
            currentState.SolutionState,
            _stateUpdater,
            _logger
       );
    }
}