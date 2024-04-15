using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Unification.Interfaces;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;

public class DisunificationGoal : IGoal
{
    private IUnificationAlgorithm _algorithm;

    public DisunificationGoal(IUnificationAlgorithm unificationAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(unificationAlgorithm, nameof(unificationAlgorithm));

        _algorithm = unificationAlgorithm;
    }

    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(state);

        if (state.CurrentGoals.Count() < 1) { throw new ArgumentException(nameof(state)); }

        DisunificationStructure evaluation;
        try
        {
            evaluation = (DisunificationStructure)state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException("Must be a disunification term!");
        }

        var substitutionMaybe = _algorithm.Unify(evaluation.Left, evaluation.Right);

        Dictionary<Variable, ISimpleTerm> substitution;
        try
        {
            substitution = substitutionMaybe.GetValueOrThrow();
        }
        catch
        {
            return
            [
                new SolverState(state.CurrentGoals.Skip(1).ToList(),state.CurrentSubstitution, state.NextInternalVariable)
            ];
        }

        return [];
    }
}
