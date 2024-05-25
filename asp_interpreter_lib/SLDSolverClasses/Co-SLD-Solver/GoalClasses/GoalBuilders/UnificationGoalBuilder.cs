namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Unification.Constructive.Unification;
using Asp_interpreter_lib.Util.ErrorHandling;

public class UnificationGoalBuilder : IGoalBuilder
{
    private readonly SolverStateUpdater _stateUpdater;
    private readonly IConstructiveUnificationAlgorithm _algorithm;
    private readonly ILogger _logger;

    public UnificationGoalBuilder(SolverStateUpdater updater, IConstructiveUnificationAlgorithm algorithm, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(updater);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _stateUpdater = updater;
        _algorithm = algorithm;
        _logger = logger;
    }

    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Must contain a structure term with two children.", nameof(goalTerm)); 
        }

        var targetEither = ConstructiveTargetBuilder.Build
        (
           goalTerm.Children.ElementAt(0),
           goalTerm.Children.ElementAt(1),
           state.Mapping
        );

        ConstructiveTarget target;
        try
        {
            target = targetEither.GetRightOrThrow();
        }
        catch
        {
            throw new ArgumentException($"{nameof(state.Mapping)} contained term bindings : {targetEither.GetLeftOrThrow().Message}");
        }

        return new UnificationGoal(_stateUpdater, target, _algorithm, state, _logger);
    }
}
