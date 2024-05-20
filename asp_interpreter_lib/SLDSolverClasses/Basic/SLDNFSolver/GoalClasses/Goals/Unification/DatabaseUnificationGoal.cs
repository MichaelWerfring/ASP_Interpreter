using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Unification.Basic.Interfaces;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.Unification;

public class DatabaseUnificationGoal : IGoal
{
    private IUnificationAlgorithm _algorithm;

    public DatabaseUnificationGoal(IUnificationAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));

        _algorithm = algorithm;
    }

    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(state);
        if (state.CurrentGoals.Count() < 1) { throw new ArgumentException(); }

        var currentGoal = state.CurrentGoals.First();

        var currentGoalStruct = currentGoal as Structure;
        if(currentGoalStruct == null) { throw new ArgumentException("Must be a structure!"); }

        foreach (var clause in database.GetPotentialUnifications(currentGoalStruct))
        {
            RenamingResult renamedClauseResult = TermFuncs.RenameClause(clause, state.NextInternalVariable);
            ISimpleTerm renamedClauseHead = renamedClauseResult.RenamedClause.First();

            var substitutionMaybe = _algorithm.Unify(currentGoal, renamedClauseHead);
            if (!substitutionMaybe.HasValue)
            {
                continue;
            }
            var substitution = substitutionMaybe.GetValueOrThrow();

            var newMapping = state.CurrentSubstitution.Union(substitution).ToDictionary(TermFuncs.GetSingletonVariableComparer());

            var nextGoals = renamedClauseResult.RenamedClause.Skip(1).Concat(state.CurrentGoals.Skip(1));
            var substitutedGoals = nextGoals.Select((term) => term.Substitute(newMapping));

            yield return new SolverState(substitutedGoals, newMapping, renamedClauseResult.NextInternalIndex);
        }
    }
}
