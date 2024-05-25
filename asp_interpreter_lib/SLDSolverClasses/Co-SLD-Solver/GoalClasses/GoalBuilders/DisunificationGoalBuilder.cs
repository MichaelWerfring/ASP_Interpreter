using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Unification.Constructive.Disunification;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class DisunificationGoalBuilder : IGoalBuilder
{
    private readonly SolverStateUpdater _stateUpdater;
    private readonly IConstructiveDisunificationAlgorithm _algorithm;
    private readonly ILogger _logger;

    public DisunificationGoalBuilder(SolverStateUpdater updater, IConstructiveDisunificationAlgorithm algorithm, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(updater);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _stateUpdater = updater;
        _algorithm = algorithm;
        _logger = logger;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));

        if (!currentState.CurrentGoals.Any())
        {
            throw new ArgumentException("Must contain at least one goal.", nameof(currentState)); 
        }

        Structure goalTerm = currentState.CurrentGoals.First();

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(currentState)); 
        }

        var targetEither = ConstructiveTargetBuilder.Build
        (
           goalTerm.Children.ElementAt(0),
           goalTerm.Children.ElementAt(1),
           currentState.SolutionState.Mapping
        );

        ConstructiveTarget target;
        try
        {
            target = targetEither.GetRightOrThrow();
        }
        catch
        {
            throw new ArgumentException
                ($"{nameof(currentState.SolutionState.Mapping)} contained term bindings : {targetEither.GetLeftOrThrow().Message}");
        }

        return new DisunificationGoal(_stateUpdater, target,_algorithm, currentState.SolutionState, _logger);
    }
}
