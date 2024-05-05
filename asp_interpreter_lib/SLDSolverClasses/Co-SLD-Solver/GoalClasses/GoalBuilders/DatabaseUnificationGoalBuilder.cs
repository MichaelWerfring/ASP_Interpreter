using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.Unification.Constructive.Unification;
using System.Collections.Immutable;

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

        return new DatabaseUnificationGoal
        (
            _algorithm,
            database,
            _mapper,
            currentState.CurrentGoals.First(),
            currentState.CurrentGoals.Skip(1).ToImmutableList(),
            currentState.SolutionState
        );
    }
}