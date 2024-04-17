using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.GoalMapping;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;

public class NafGoal : IGoal
{
    private FunctorTableRecord _functorTable;

    public NafGoal(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        _functorTable = functorTable;
    }

    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(state);

        if (state.CurrentGoals.Count() < 1) { throw new ArgumentException(nameof(state)); }

        Structure naf;
        try
        {
            naf = (Structure)state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException("Must be a structure.");
        }
        if (naf.Children.Count() != 1)
        {
            throw new ArgumentException(nameof(state));
        }

        var solver = new AdvancedSLDSolver(database, _functorTable);
        bool foundSolution = false;
        solver.SolutionFound += ((_, _) => foundSolution = true);

        solver.Solve([naf.Children.ElementAt(0)]);

        if (foundSolution)
        {
            return [];
        }

        return [new SolverState(state.CurrentGoals.Skip(1).ToList(), state.CurrentSubstitution, state.NextInternalVariable)];
    }
}
