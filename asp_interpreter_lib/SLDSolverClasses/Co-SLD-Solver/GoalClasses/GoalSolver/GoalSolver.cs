// <copyright file="GoalSolver.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;

/// <summary>
/// A solver for solving multiple goals.
/// </summary>
public class GoalSolver
{
    private readonly CoSLDGoalMapper goalMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoalSolver"/> class.
    /// </summary>
    /// <param name="goalMapper">A goal mapper for determining the kind of goal of a goal structure.</param>
    public GoalSolver(CoSLDGoalMapper goalMapper)
    {
        ArgumentNullException.ThrowIfNull(goalMapper, nameof(goalMapper));

        this.goalMapper = goalMapper;
    }

    /// <summary>
    /// Attempts to solve a goal, contained in an input state.
    /// </summary>
    /// <param name="inputState">The input solver state.</param>
    /// <returns>An enumeration of solutions.</returns>
    public IEnumerable<GoalSolution> SolveGoals(CoSldSolverState inputState)
    {
        if (!inputState.CurrentGoals.Any())
        {
            yield return new GoalSolution(
             inputState.SolutionState.CHS,
             inputState.SolutionState.Mapping,
             inputState.SolutionState.Callstack,
             inputState.SolutionState.NextInternalVariableIndex);

            yield break;
        }

        ICoSLDGoal goal = this.goalMapper.GetGoal(inputState);

        // for each way the goal can be satisfied..
        foreach (GoalSolution solution in goal.TrySatisfy())
        {
            CoSldSolverState nextState = this.UpdateAfterGoalFulfilled(inputState, solution);

            // yield return all the ways the rest of the goals can be satisfied
            foreach (GoalSolution resolution in this.SolveGoals(nextState))
            {
                yield return resolution;
            }
        }
    }

    private CoSldSolverState UpdateAfterGoalFulfilled(CoSldSolverState inputState, GoalSolution goalSolution)
    {
        var goalTail = inputState.CurrentGoals.Skip(1);

        if (goalTail.Any())
        {
            var substitutedNextGoal = goalSolution.ResultMapping.ApplySubstitution(goalTail.First());

            goalTail = goalTail.Skip(1).Prepend(substitutedNextGoal);
        }

        return new CoSldSolverState(
            goalTail,
            new SolutionState(
                goalSolution.Stack,
                goalSolution.ResultSet,
                goalSolution.ResultMapping,
                goalSolution.NextInternalVariable));
    }
}