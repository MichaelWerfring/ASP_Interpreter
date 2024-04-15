using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.Events;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;
using asp_interpreter_lib.SLDSolverClasses.SubstitutionPostProcessing;
using asp_interpreter_lib.SLDSolverClasses.VariableRenaming;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver;

public class AdvancedSLDSolver
{
    private SubstitutionPostProcessor _postProcessor = new SubstitutionPostProcessor();

    private IDatabase _database;
    private GoalResolver _goalSolver;

    public AdvancedSLDSolver(IDatabase database, GoalResolver goalSolver)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(goalSolver);

        _database = database;
        _goalSolver = goalSolver;
    }

    public event EventHandler<SolutionFoundEventArgs>? SolutionFound;

    public void Solve(IEnumerable<ISimpleTerm> goals)
    {
        ArgumentNullException.ThrowIfNull(goals);
        if(goals.Any((term) => term == null))
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
        
        foreach(SolverState branch in branches)
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
