using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.Unification.Constructive.Unification;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class DatabaseUnificationGoalBuilder : IGoalBuilder
{
    private readonly CoSLDGoalMapper _mapper;
    private readonly IConstructiveUnificationAlgorithm _algorithm;

    public DatabaseUnificationGoalBuilder(CoSLDGoalMapper mapper, IConstructiveUnificationAlgorithm unificationAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(unificationAlgorithm, nameof(unificationAlgorithm));

        _mapper = mapper;
        _algorithm = unificationAlgorithm;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        if (currentState.CurrentGoals.Count() < 1) { throw new ArgumentException(nameof(currentState)); }

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
                new CHSChecker(new FunctorTableRecord(), new GoalSolver(_mapper, database)),
                new CallstackChecker(new FunctorTableRecord())
            ),
            new DatabaseUnifier(_algorithm, database),
            new GoalSolver(_mapper,database), 
            goalStruct,
            currentState.SolutionState
       );
    }
}