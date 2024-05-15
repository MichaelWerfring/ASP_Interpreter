using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class PredicateGoalBuilder : IGoalBuilder
{
    private readonly ILogger _logger;

    private readonly CoSLDGoalMapper _mapper;

    private readonly IConstructiveUnificationAlgorithm _algorithm;

    private readonly PredicateGoalStateUpdater _stateUpdater;

    public PredicateGoalBuilder
    (
        CoSLDGoalMapper mapper, 
        IConstructiveUnificationAlgorithm unificationAlgorithm,
        PredicateGoalStateUpdater updater,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(unificationAlgorithm, nameof(unificationAlgorithm));
        ArgumentNullException.ThrowIfNull (updater, nameof(updater));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _logger = logger;
        _mapper = mapper;
        _stateUpdater = updater;
        _algorithm = unificationAlgorithm;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        if (!currentState.CurrentGoals.Any())
        {
            throw new ArgumentException(nameof(currentState)); 
        }

        ISimpleTerm goalTerm = currentState.CurrentGoals.ElementAt(0);

        Structure goalStruct;
        try
        {
            goalStruct = (Structure) goalTerm;
        }
        catch
        {
            throw new ArgumentException("Predicate goal must be a structure." ,nameof(currentState));
        }

        return new PredicateGoal
        (
            new CoinductiveChecker
            (
                new CHSChecker(new FunctorTableRecord(), new GoalSolver(_mapper, database, _logger)),
                new CallstackChecker(new FunctorTableRecord())
            ),
            new DatabaseUnifier(_algorithm, database),
            new GoalSolver(_mapper,database, _logger), 
            goalStruct,
            currentState.SolutionState,
            _stateUpdater,
            _logger
       );
    }
}