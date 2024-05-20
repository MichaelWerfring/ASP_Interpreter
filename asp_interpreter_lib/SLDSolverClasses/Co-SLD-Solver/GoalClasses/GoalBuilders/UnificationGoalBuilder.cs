using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Target.Builder;
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
            _logger.LogError("Failed to build unification goal: state did not contain any goals.");
            throw new ArgumentException("Must contain at least one goal!", nameof(currentState)); 
        }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure disunificationStruct || disunificationStruct.Children.Count != 2)
        {
            _logger.LogError($"Failed to build unification goal: " +
                $"Goalterm {goalTerm} was not of type struct or did not contain 2 children.");
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
            _logger.LogError($"Failed to build unification goal: {targetMaybe.GetLeftOrThrow().Message}");
            throw new ArgumentException($"{nameof(currentState.SolutionState.Mapping)} contained term bindings : {targetMaybe.GetLeftOrThrow().Message}");
        }

        return new UnificationGoal(target, _algorithm, currentState.SolutionState, _logger);
    }
}
