﻿using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Unification.Basic.Interfaces;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.ClauseRenamer;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.Unification;

public class DatabaseUnificationGoal : IGoal
{
    private ClauseVariableRenamer _variableRenamer = new ClauseVariableRenamer();

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
            var substitutedGoals = nextGoals.Select((term) => term.Substitute(newMapping));

            yield return new SolverState(substitutedGoals, newMapping, renamedClauseResult.NextInternalIndex);
        }
    }
}