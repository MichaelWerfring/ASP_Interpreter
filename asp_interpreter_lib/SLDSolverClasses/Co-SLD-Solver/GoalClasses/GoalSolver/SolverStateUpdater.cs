using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

internal class SolverStateUpdater
{
    private VariableMappingSimplifier _simplifier = new VariableMappingSimplifier();

    public CoSldSolverState GetNewStateWithSubstitutedCurrentGoal(CoSldSolverState inputState)
    {
        ArgumentNullException.ThrowIfNull(inputState, nameof(inputState));
        if (!inputState.CurrentGoals.Any())
        { 
            throw new ArgumentException("Must contain at least one goal.",nameof(inputState)); 
        }

        IEnumerable<ISimpleTerm> substitutedGoal =
        [
            inputState.SolutionState.Mapping.ApplySubstitution
            (inputState.CurrentGoals.First())
        ];

        return new CoSldSolverState
        (
            substitutedGoal.Concat(inputState.CurrentGoals.Skip(1)),
            new SolutionState
            (
                inputState.SolutionState.Stack,
                inputState.SolutionState.Set,
                inputState.SolutionState.Mapping,
                inputState.SolutionState.NextInternalVariableIndex
            )
        );
    }

    public CoSldSolverState CreateNewStateFromGoalSolution(CoSldSolverState oldState, GoalSolution solutionToUpdateWith)
    {
        ArgumentNullException.ThrowIfNull(oldState, nameof(oldState));
        ArgumentNullException.ThrowIfNull(solutionToUpdateWith, nameof(solutionToUpdateWith));
        if (!oldState.CurrentGoals.Any())
        {
            throw new ArgumentException("Must contain at least one goal.", nameof(oldState));
        }

        var simplifiedMapping = _simplifier.Simplify(solutionToUpdateWith.ResultMapping);

        return new CoSldSolverState
        (
            oldState.CurrentGoals.Skip(1),
            new SolutionState
            (
                oldState.SolutionState.Stack,
                solutionToUpdateWith.ResultSet,
                simplifiedMapping,
                solutionToUpdateWith.NextInternalVariable
            )
        );
    }
}