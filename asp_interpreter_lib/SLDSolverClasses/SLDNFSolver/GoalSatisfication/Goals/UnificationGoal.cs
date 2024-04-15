using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.VariableRenaming;
using asp_interpreter_lib.Unification.Interfaces;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;

public class UnificationGoal : IGoal
{
    private IUnificationAlgorithm _algorithm;
    private VariableSubstituter _substituter = new VariableSubstituter();

    public UnificationGoal(IUnificationAlgorithm unificationAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(unificationAlgorithm, nameof(unificationAlgorithm));

        _algorithm = unificationAlgorithm;
    }

    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(state);

        if (state.CurrentGoals.Count() < 1) { throw new ArgumentException(nameof(state)); }

        UnificationStructure evaluation;
        try
        {
            evaluation = (UnificationStructure)state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException("Must be a unification term!");
        }

        var substitutionMaybe = _algorithm.Unify(evaluation.Left, evaluation.Right);

        Dictionary<Variable, ISimpleTerm> substitution;
        try
        {
            substitution = substitutionMaybe.GetValueOrThrow();
        }
        catch
        {
            return [];
        }

        var newGoals = state.CurrentGoals.Skip(1).Select((term) => _substituter.Substitute(term, substitution));

        return
        [
            new SolverState
                (
                    newGoals,
                    state.CurrentSubstitution.Union(substitution).ToDictionary(new VariableComparer()),
                    state.NextInternalVariable
                )
        ];
    }
}
