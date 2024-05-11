using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Basic.Events;
using asp_interpreter_lib.SLDSolverClasses.Basic.PostProcessing;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver;

public class AdvancedSLDSolver
{
    private SubstitutionPostProcessor _postProcessor = new SubstitutionPostProcessor();

    private GoalResolver _goalSolver;
    private IDatabase _database;

    public AdvancedSLDSolver(IDatabase database, FunctorTableRecord specialFunctors)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(specialFunctors);

        _database = database;
        _goalSolver = new GoalResolver(specialFunctors);
    }

    public event EventHandler<SolutionFoundEventArgs>? SolutionFound;

    public void Solve(IEnumerable<ISimpleTerm> goals)
    {
        ArgumentNullException.ThrowIfNull(goals);
        if (goals.Any((term) => term == null))
        {
            throw new ArgumentNullException("Must not contain nulls");
        }

        Resolve
        (
            new SolverState
            (
                goals,
                new Dictionary<Variable, ISimpleTerm>(new VariableComparer()),
                0
            )
        );
    }

    private void Resolve(SolverState state)
    {
        if (state.CurrentGoals.Count() == 0)
        {
            SimplifyAndTrigger(state);
            return;
        }

        var branches = _goalSolver.Solve(state, _database);

        foreach (SolverState branch in branches)
        {
            Resolve(branch);
        }
    }

    private void SimplifyAndTrigger(SolverState state)
    {
        var finalMapping = _postProcessor.Simplify(state.CurrentSubstitution);

        SolutionFound?.Invoke(this, new SolutionFoundEventArgs(finalMapping));
    }
}
