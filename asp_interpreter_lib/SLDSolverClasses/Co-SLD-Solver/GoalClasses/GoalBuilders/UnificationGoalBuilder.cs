using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class UnificationGoalBuilder : IGoalBuilder
{
    private readonly IConstructiveUnificationAlgorithm _algorithm;
    private readonly ConstructiveTargetBuilder _targetBuilder;

    public UnificationGoalBuilder(IConstructiveUnificationAlgorithm algorithm, ConstructiveTargetBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(builder);

        _algorithm = algorithm;
        _targetBuilder = builder;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        if (currentState.CurrentGoals.Count() == 0)
        { throw new ArgumentException("Must contain at least one term!", nameof(currentState.CurrentGoals)); }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure disunificationStruct || disunificationStruct.Children.Count() != 2)
        { throw new ArgumentException("Must contain a structure term with two children.", nameof(currentState.CurrentGoals)); }

        ConstructiveTarget target = _targetBuilder.Build
           (
              disunificationStruct.Children.ElementAt(0),
              disunificationStruct.Children.ElementAt(1),
              currentState.SolutionState.Mapping
           );

        return new UnificationGoal(target, _algorithm, currentState.SolutionState);
    }
}
