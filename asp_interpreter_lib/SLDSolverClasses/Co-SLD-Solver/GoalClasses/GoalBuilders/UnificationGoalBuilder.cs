using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class UnificationGoalBuilder : IGoalBuilder
{
    private readonly IConstructiveUnificationAlgorithm _algorithm;

    private readonly ILogger _logger;

    public UnificationGoalBuilder(IConstructiveUnificationAlgorithm algorithm, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _algorithm = algorithm;
        _logger = logger;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));

        if (!currentState.CurrentGoals.Any())
        {
            throw new ArgumentException("Must contain at least one goal!", nameof(currentState)); 
        }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure disunificationStruct || disunificationStruct.Children.Count != 2)
        { 
            throw new ArgumentException("Must contain a structure term with two children.", nameof(currentState)); 
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
            target = targetMaybe.GetRightOrThrow();
        }
        catch
        {
            throw new ArgumentException
                ($"{nameof(currentState.SolutionState.Mapping)} contained term bindings " +
                $"for variables in unification goal term {goalTerm}",nameof(currentState));
        }

        return new UnificationGoal(target, _algorithm, currentState.SolutionState, _logger);
    }
}
