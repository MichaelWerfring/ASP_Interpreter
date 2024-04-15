using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.VariableRenaming;
using asp_interpreter_lib.Unification.Interfaces;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;

public class DatabaseUnificationGoal : IGoal
{
    private ClauseVariableRenamer _variableRenamer = new ClauseVariableRenamer();
    private VariableSubstituter _substituter = new VariableSubstituter();

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

        foreach (var clause in database.GetMatchingClauses(currentGoal))
        {
            RenamingResult renamedClauseResult = _variableRenamer.RenameVariables(clause, state.NextInternalVariable);
            ISimpleTerm renamedClauseHead = renamedClauseResult.RenamedClause.First();

            var substitutionMaybe = _algorithm.Unify(currentGoal, renamedClauseHead);
            if (!substitutionMaybe.HasValue)
            {
                continue;
            }
            var substitution = substitutionMaybe.GetValueOrThrow();

            var newMapping = state.CurrentSubstitution.Union(substitution).ToDictionary(new VariableComparer());

            var nextGoals = renamedClauseResult.RenamedClause.Skip(1).Concat(state.CurrentGoals.Skip(1));
            var substitutedGoals = nextGoals.Select((term) => _substituter.Substitute(term, newMapping));

            yield return new SolverState(substitutedGoals, newMapping, renamedClauseResult.NextInternalIndex);
        }
    }
}
