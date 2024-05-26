// <copyright file="PredicateGoalStateUpdater.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

/// <summary>
/// A class that is used to update the solver state during predicate goal execution.
/// </summary>
public class PredicateGoalStateUpdater
{
    private readonly SolverStateUpdater updater;

    /// <summary>
    /// Initializes a new instance of the <see cref="PredicateGoalStateUpdater"/> class.
    /// </summary>
    /// <param name="updater">An updater for updating state after unification and subgoal solving.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="updater"/> is null.</exception>
    public PredicateGoalStateUpdater(SolverStateUpdater updater)
    {
        ArgumentNullException.ThrowIfNull(updater, nameof(updater));

        this.updater = updater;
    }

    /// <summary>
    /// Updates the state after unification.
    /// </summary>
    /// <param name="inputSet">The input chs that will be updated.</param>
    /// <param name="inputStack">The input callstack.</param>
    /// <param name="unifyingMapping">The mapping after unification.</param>
    /// <param name="renamedClause">The renamed clause after unification.</param>
    /// <param name="constrainedTarget">The current predicate.
    /// This should always be the predicate after it comes out of the coinductive check.</param>
    /// <param name="nextInternal">The next internal variable index, after clause renaming.</param>
    /// <returns>The new solver state.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="inputSet"/> is null,
    /// ..<paramref name="inputStack"/> is null,
    /// ..<paramref name="unifyingMapping"/> is null,
    /// ..<paramref name="renamedClause"/> is null,
    /// <paramref name="constrainedTarget"/> is null.</exception>
    public CoSldSolverState BuildStateForSolvingBodyGoals(
        CoinductiveHypothesisSet inputSet,
        CallStack inputStack,
        VariableMapping unifyingMapping,
        IEnumerable<Structure> renamedClause,
        Structure constrainedTarget,
        int nextInternal)
    {
        ArgumentNullException.ThrowIfNull(inputSet);
        ArgumentNullException.ThrowIfNull(inputStack);
        ArgumentNullException.ThrowIfNull(unifyingMapping);
        ArgumentNullException.ThrowIfNull(renamedClause);
        ArgumentNullException.ThrowIfNull(constrainedTarget);

        // update chs by updating all the variables in it.
        var newCHS = this.updater.UpdateCHS(inputSet, unifyingMapping);

        // update callstack by pushing target onto stack and updating all the variables in it.
        var newCallstack = this.updater.UpdateCallstack(inputStack.Push(constrainedTarget), unifyingMapping);

        // substitute the next goal, if there is one.
        var nextGoals = renamedClause.Skip(1);
        if (nextGoals.Any())
        {
            var substitutedNextGoal = unifyingMapping.ApplySubstitution(nextGoals.First());
            nextGoals = nextGoals.Skip(1).Prepend(substitutedNextGoal);
        }

        return new CoSldSolverState(
            nextGoals,
            new SolutionState(
                newCallstack,
                newCHS,
                unifyingMapping,
                nextInternal));
    }

    /// <summary>
    /// Constructs a goal solution after coinductive success.
    /// </summary>
    /// <param name="state">The predicate goal input solution state.</param>
    /// <param name="constrainedMapping">The constrainedMapping after coinductive checking.</param>
    /// <returns>A goal solution to return.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="state"/> is null,
    /// ..<paramref name="constrainedMapping"/> is null.</exception>
    public GoalSolution ConstructCoinductiveSuccessSolution(SolutionState state, VariableMapping constrainedMapping)
    {
        ArgumentNullException.ThrowIfNull(constrainedMapping);
        ArgumentNullException.ThrowIfNull(state);

        return new GoalSolution(
            this.updater.UpdateCHS(state.CHS, constrainedMapping),
            constrainedMapping,
            this.updater.UpdateCallstack(state.Callstack, constrainedMapping),
            state.NextInternalVariableIndex);
    }

    /// <summary>
    /// Updates a subgoal solution by adding target to chs and popping the latest item from the callstack.
    /// </summary>
    /// <param name="subgoalSolution">The subgoal solution.</param>
    /// <param name="target">The predicate goal structure.</param>
    /// <returns>The updated goal solution.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="subgoalSolution"/> is null,
    /// ..<paramref name="target"/> is null.</exception>
    public GoalSolution UpdateGoalSolution(GoalSolution subgoalSolution, Structure target)
    {
        ArgumentNullException.ThrowIfNull(subgoalSolution);
        ArgumentNullException.ThrowIfNull(target);

        var substitutedTarget = subgoalSolution.ResultMapping.ApplySubstitution(target);

        var entry = new CHSEntry(substitutedTarget, true);

        var newCHS = subgoalSolution.ResultSet.Add(entry);

        return new GoalSolution(
            newCHS,
            subgoalSolution.ResultMapping,
            subgoalSolution.Stack.Pop(),
            subgoalSolution.NextInternalVariable);
    }
}