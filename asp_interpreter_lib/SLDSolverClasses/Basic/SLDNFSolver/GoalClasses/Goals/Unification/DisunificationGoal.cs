using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Unification.Basic.Interfaces;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.Unification;

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

        Structure disunification;
        try
        {
            disunification = (Structure)state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException("Must be a structure.");
        }
        if (disunification.Children.Count() != 2)
        {
            throw new ArgumentException(nameof(state));
        }

        var substitutionMaybe = _algorithm.Unify(disunification.Children.ElementAt(0), disunification.Children.ElementAt(1));

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
