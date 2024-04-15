using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.GoalMapping;
using asp_interpreter_lib.Unification.Robinson;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;

public class NafGoal : IGoal
{
    private GoalResolver _resolver;

    public NafGoal(GoalResolver goalResolver)
    {
        ArgumentNullException.ThrowIfNull(goalResolver);

        _resolver = goalResolver;
    }

    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(state);

        if (state.CurrentGoals.Count() < 1) { throw new ArgumentException(nameof(state)); }

        Naf term;
        try
        {
            term = (Naf)state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException("Must be a naf term!");
        }

        var solver = new AdvancedSLDSolver(database, _resolver);
        bool foundSolution = false;
        solver.SolutionFound += ((_, _) => foundSolution = true);

        solver.Solve([term.Term]);

        if (foundSolution)
        {
            return [];
        }

        return [new SolverState(state.CurrentGoals.Skip(1).ToList(), state.CurrentSubstitution, state.NextInternalVariable)];
    }
}
