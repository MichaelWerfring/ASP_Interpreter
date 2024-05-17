using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class ForallGoal : ICoSLDGoal
{
    private readonly TransitiveVariableMappingResolver _resolver = new(true);

    private readonly GoalSolver _solver;

    private readonly Variable _variable;

    private readonly ISimpleTerm _goalTerm;

    private readonly SolutionState _solutionState;

    private readonly ILogger _logger;

    public ForallGoal
    (
        GoalSolver solver,
        Variable variable,
        ISimpleTerm goalTerm,
        SolutionState solutionState,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(goalTerm, nameof(goalTerm));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _solver = solver;
        _variable = variable;
        _goalTerm = goalTerm;
        _solutionState = solutionState;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        var initialState = new CoSldSolverState([_goalTerm], _solutionState);

        foreach (GoalSolution initialForallSolution in _solver.SolveGoals(initialState))
        {
            // get binding. transitive resolving is necessary because through unification,
            // you could have something like X -> Y -> \={1,2}, where
            IVariableBinding? mappingForForallVariable = null;
            try
            {
                mappingForForallVariable = _resolver.Resolve(_variable, initialForallSolution.ResultMapping);
            }
            catch 
            {
                continue;
            }

            // if no binding, succeed.
            if (mappingForForallVariable == null)
            {
                yield return initialForallSolution;
                yield break;
            }

            // if bound to term, fail.
            if (mappingForForallVariable is TermBinding)
            {
                continue;
            }

            // now we know it is a prohibited values binding.
            var prohibitedValuesForVariable = (ProhibitedValuesBinding)mappingForForallVariable;

            // if no prohibited values(unconstrained), then succeed.
            if (prohibitedValuesForVariable.ProhibitedValues.Count == 0)
            {
                yield return initialForallSolution;
                yield break;
            }

            // construct new goals where variable in goalTerm is substituted by each prohibited value of variable.
            var constraintSubstitutedGoals = prohibitedValuesForVariable.ProhibitedValues
            .Select(prohibitedTerm => _goalTerm.Substitute
            (
                new Dictionary<Variable, ISimpleTerm>(new VariableComparer())
                {
                    { _variable, prohibitedTerm }
                })
            );

            // construct new solver state
            var initialSolvingState = new CoSldSolverState
            (
                constraintSubstitutedGoals,
                new SolutionState
                (
                    initialForallSolution.Stack,
                    initialForallSolution.ResultSet,
                    initialForallSolution.ResultMapping, 
                    initialForallSolution.NextInternalVariable
                )
            );

            IEnumerable<GoalSolution> solutions = _solver.SolveGoals(initialSolvingState);

            foreach ( GoalSolution solution in solutions ) 
            {
                 yield return solution;
            }
        }
    }
}
