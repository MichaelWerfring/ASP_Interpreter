using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class GoalSolver
{
    private readonly VariableMappingSimplifier _simplifier = new();

    private readonly CoSLDGoalMapper _goalMapper;

    private readonly IDatabase _database;

    public GoalSolver(CoSLDGoalMapper goalMapper, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(goalMapper, nameof(goalMapper));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        _goalMapper = goalMapper;
        _database = database;
    }

    public IEnumerable<GoalSolution> SolveGoals(CoSldSolverState inputState)
    {      
        if (inputState.CurrentGoals.Count() == 0)
        {
            yield return new GoalSolution
            (
                inputState.SolutionState.CurrentSet,
                inputState.SolutionState.CurrentMapping,
                inputState.SolutionState.NextInternalVariableIndex
            );
            yield break;
        }

        var stateWithSubstitutedGoals = Update(inputState);

        var goalToSolveMaybe = _goalMapper.GetGoal(stateWithSubstitutedGoals, _database);

        if (!goalToSolveMaybe.HasValue) { yield break; }

        var goal = goalToSolveMaybe.GetValueOrThrow();

        // for each way the goal can be satisified..
        foreach(var result in goal.TrySatisfy())
        {
            var newState = CreateNewStateFromGoalSolution(inputState, result);

            // yield return all the ways the rest of the goals can be satisfied
            foreach (var resolution in SolveGoals(newState))
            {
                yield return resolution; 
            }
        }
    }

    private CoSldSolverState Update(CoSldSolverState inputState)
    {
        var substitutedGoals = inputState.CurrentGoals
            .Select(inputState.SolutionState.CurrentMapping.ApplySubstitution);

        return new CoSldSolverState
        (
            substitutedGoals,
            new SolutionState
            (
                inputState.SolutionState.CurrentStack,
                inputState.SolutionState.CurrentSet,
                inputState.SolutionState.CurrentMapping,
                inputState.SolutionState.NextInternalVariableIndex
            )
        );
    }

    private CoSldSolverState CreateNewStateFromGoalSolution(CoSldSolverState oldState, GoalSolution solutionToUpdateWith)
    {
        var simplifiedMapping = _simplifier.Simplify(solutionToUpdateWith.ResultMapping);

        return new CoSldSolverState
        (
            oldState.CurrentGoals.Skip(1),
            new SolutionState
            (
                oldState.SolutionState.CurrentStack,
                solutionToUpdateWith.ResultSet,
                simplifiedMapping,
                solutionToUpdateWith.NextInternalVariable
            )
        );
    }
}
