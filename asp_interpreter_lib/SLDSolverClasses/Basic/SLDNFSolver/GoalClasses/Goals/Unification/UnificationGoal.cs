using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Unification.Basic.Interfaces;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.Unification;

public class UnificationGoal : IGoal
{
    private IUnificationAlgorithm _algorithm;

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
        Structure evaluation;
        try
        {
            evaluation = (Structure)state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException(nameof(state));
        }
        if (evaluation.Children.Count() != 2) { throw new ArgumentException(nameof(state)); }

        var substitutionMaybe = _algorithm.Unify(evaluation.Children.ElementAt(0), evaluation.Children.ElementAt(1));

        Dictionary<Variable, ISimpleTerm> substitution;
        try
        {
            substitution = substitutionMaybe.GetValueOrThrow();
        }
        catch
        {
            return [];
        }

        var newGoals = state.CurrentGoals.Skip(1).Select((term) => term.Substitute(substitution));

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
