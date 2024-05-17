using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class UnificationGoalBuilder : IGoalBuilder
{
    private readonly IConstructiveUnificationAlgorithm _algorithm;

    public UnificationGoalBuilder(IConstructiveUnificationAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm);

        _algorithm = algorithm;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        if (!currentState.CurrentGoals.Any())
        {
            throw new ArgumentException("Must contain at least one goal!", nameof(currentState.CurrentGoals)); 
        }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure disunificationStruct || disunificationStruct.Children.Count() != 2)
        { 
            throw new ArgumentException("Must contain a structure term with two children.", nameof(currentState.CurrentGoals)); 
        }

        var targetMaybe = ConstructiveTargetBuilder.Build
        (
           disunificationStruct.Children.ElementAt(0),
           disunificationStruct.Children.ElementAt(1),
           currentState.SolutionState.Mapping
        );

        ConstructiveTarget target;
        try
        {
            target = targetMaybe.GetValueOrThrow();
        }
        catch
        {
            throw new ArgumentException
                ($"{nameof(currentState.SolutionState.Mapping)} contained term bindings " +
                $"for variables in unification goal term {goalTerm}",nameof(currentState));
        }

        return new UnificationGoal(target, _algorithm, currentState.SolutionState);
    }
}
