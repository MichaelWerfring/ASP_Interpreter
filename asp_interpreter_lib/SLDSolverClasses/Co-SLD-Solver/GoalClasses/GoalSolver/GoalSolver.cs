using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class GoalSolver
{
    private readonly CoSLDGoalMapper _goalMapper;
    private readonly ILogger _logger;

    public GoalSolver(CoSLDGoalMapper goalMapper, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(goalMapper, nameof(goalMapper));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _goalMapper = goalMapper;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> SolveGoals(CoSldSolverState inputState)
    {      
        if (!inputState.CurrentGoals.Any())
        {
            yield return new GoalSolution
            (
                inputState.SolutionState.CHS,
                inputState.SolutionState.Mapping,
                inputState.SolutionState.Callstack,
                inputState.SolutionState.NextInternalVariableIndex
            );
            yield break;
        }

        IOption<ICoSLDGoal> goalToSolveMaybe = _goalMapper.GetGoal(inputState);

        if (!goalToSolveMaybe.HasValue)
        {
            yield break;
        }

        ICoSLDGoal goal = goalToSolveMaybe.GetValueOrThrow();
        
        // for each way the goal can be satisfied..
        foreach (GoalSolution solution in goal.TrySatisfy())
        {
            CoSldSolverState nextState = UpdatAfterGoalFulfilled(inputState, solution);
         
            // yield return all the ways the rest of the goals can be satisfied
            foreach (GoalSolution resolution in SolveGoals(nextState))
            {
                yield return resolution;
            }
        }
    }

    private CoSldSolverState UpdatAfterGoalFulfilled(CoSldSolverState inputState, GoalSolution goalSolution)
    {
        var goalTail = inputState.CurrentGoals.Skip(1);

        if (goalTail.Any())
        {
            var substitutedNextGoal = goalSolution.ResultMapping.ApplySubstitution(goalTail.First());

            goalTail = goalTail.Skip(1).Prepend(substitutedNextGoal);
        }

        return new CoSldSolverState
        (
            goalTail,
            new SolutionState
            (
                goalSolution.Stack,
                goalSolution.ResultSet,
                goalSolution.ResultMapping,
                goalSolution.NextInternalVariable
            )
        );
    }
}