using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class DatabaseUnificationGoalBuilder : IGoalBuilder
{
    private readonly ILogger _logger;
    private readonly CoSLDGoalMapper _mapper;
    private readonly IConstructiveUnificationAlgorithm _algorithm;

    public DatabaseUnificationGoalBuilder(
        CoSLDGoalMapper mapper, 
        IConstructiveUnificationAlgorithm unificationAlgorithm,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(unificationAlgorithm, nameof(unificationAlgorithm));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _logger = logger;
        _mapper = mapper;
        _algorithm = unificationAlgorithm;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        if (!currentState.CurrentGoals.Any()) { throw new ArgumentException(nameof(currentState)); }

        var goalTerm = currentState.CurrentGoals.ElementAt(0);
        Structure goalStruct;
        try
        {
            goalStruct = (Structure) goalTerm;
        }
        catch
        {
            throw;
        }

        return new DatabaseUnificationGoal
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
            _logger
       );
    }
}