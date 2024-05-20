using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;

public class PredicateGoalStateUpdater
{
    private readonly SolverStateUpdater _updater;

    public PredicateGoalStateUpdater(SolverStateUpdater updater)
    {
        ArgumentNullException.ThrowIfNull(updater, nameof(updater));

        _updater = updater;
    }

    /// <summary>
    /// Adds the current goal to the chs with false flag, updates all the entries.
    /// Then does the same with callstack.
    /// Then constructs the mapping for the next solver state.
    /// Then substitutes
    /// </summary>
    public CoSldSolverState BuildStateForSolvingBodyGoals
    (
        CoinductiveHypothesisSet inputSet,
        CallStack inputStack,
        VariableMapping unifyingMapping,
        IEnumerable<ISimpleTerm> renamedClause, 
        Structure constrainedTarget,
        int nextInternal
    )
    {
        // update chs by updating all the variables in it.
        var newCHS =  _updater.UpdateCHS(inputSet, unifyingMapping);

        // update callstack by pushing target onto stack and updating all the variables in it.
        var newCallstack = _updater.UpdateCallstack(inputStack.Push(constrainedTarget), unifyingMapping);
        
        // substitute the next goal, if there is one.
        var nextGoals = renamedClause.Skip(1);
        if (nextGoals.Any())
        {
            var substitutedNextGoal = unifyingMapping.ApplySubstitution(nextGoals.First());
            nextGoals = nextGoals.Skip(1).Prepend(substitutedNextGoal);
        }

        return new CoSldSolverState
        (
            nextGoals,
            new SolutionState
            (
                newCallstack,
                newCHS,
                unifyingMapping,
                nextInternal
            )
        );
    }

    public GoalSolution ConstructCoinductiveSuccessSolution(SolutionState state, VariableMapping result)
    {
        return new GoalSolution
        (
            _updater.UpdateCHS(state.CHS, result),
            result,
            _updater.UpdateCallstack(state.Callstack, result),
            state.NextInternalVariableIndex
        );
    }

    /// <summary>
    /// updates subgoal solution by adding target to chs and popping the latest item from callstack.
    /// </summary>
    public GoalSolution UpdateGoalSolution(GoalSolution subgoalSolution, Structure target)
    {
        var substitutedTarget = subgoalSolution.ResultMapping.ApplySubstitution(target);

        var entry = new CHSEntry(substitutedTarget, true);

        var newCHS = subgoalSolution.ResultSet.Add(entry);

        return new GoalSolution
        (
            newCHS,
            subgoalSolution.ResultMapping,
            subgoalSolution.Stack.Pop(),
            subgoalSolution.NextInternalVariable
        );
    }
}
