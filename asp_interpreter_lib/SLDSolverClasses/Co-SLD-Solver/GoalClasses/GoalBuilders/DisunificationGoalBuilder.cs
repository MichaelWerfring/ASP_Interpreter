using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class DisunificationGoalBuilder : IGoalBuilder
{
    private readonly IConstructiveDisunificationAlgorithm _algorithm;
    private readonly ILogger _logger;

    public DisunificationGoalBuilder(IConstructiveDisunificationAlgorithm algorithm, ILogger logger)
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
            throw new ArgumentException("Must contain at least one goal.", nameof(currentState)); 
        }

        ISimpleTerm goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure disunificationStruct || disunificationStruct.Children.Count != 2)
        { 
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(currentState)); 
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
            (
                $"Mapping contained term bindings for variables in disunification goal term {goalTerm}",
                nameof(currentState)
            );
        }

        return new DisunificationGoal(target,_algorithm, currentState.SolutionState, _logger);
    }
}
