using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;

public class CoinductiveSLDSolver
{
    private readonly GoalSolver _goalSolver;

    public CoinductiveSLDSolver(IDatabase database, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        _goalSolver = new GoalSolver(new CoSLDGoalMapper(functors), database);
    }

    public IEnumerable<CoSLDSolution> Solve(IEnumerable<ISimpleTerm> query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var initialSolverState = new CoSldSolverState
        (
            query,
            new SolutionState
            (
                new CallStack(ImmutableStack.Create<ISimpleTerm>()),
                new CoinductiveHypothesisSet(),
                new VariableMapping(),
                0
            )
        );

        foreach(var querySolution in _goalSolver.SolveGoals(initialSolverState))
        {
            yield return new CoSLDSolution(querySolution.ResultSet, querySolution.ResultMapping);
        }
    }
}
