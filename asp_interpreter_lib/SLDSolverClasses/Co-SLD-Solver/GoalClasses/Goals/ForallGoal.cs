using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Util;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class ForallGoal : ICoSLDGoal
{
    private readonly GoalSolver _solver;

    private readonly Variable _variable;

    private readonly ISimpleTerm _goalTerm;

    private readonly SolutionState _inputState;

    private readonly ILogger _logger;

    public ForallGoal
    (
        GoalSolver solver,
        Variable variable,
        ISimpleTerm goalTerm,
        SolutionState state,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(goalTerm, nameof(goalTerm));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _solver = solver;
        _variable = variable;
        _goalTerm = goalTerm;
        _inputState = state;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve forall goal with var {_variable}, goal {_goalTerm}");
        _logger.LogTrace($"Input state is: {_inputState}");

        var initialState = new CoSldSolverState([_goalTerm], _inputState);

        foreach (GoalSolution initialForallSolution in _solver.SolveGoals(initialState))
        {
            _logger.LogInfo("Initial forall solution solution found!");
            _logger.LogTrace($"{initialForallSolution}");

            // get binding. transitive resolving is necessary because through unification,
            // you could have something like X -> Y -> \={1,2}, where
            IVariableBinding? mappingForForallVariable = null;
            try
            {
                mappingForForallVariable = initialForallSolution.ResultMapping.Resolve(_variable, true);
            }
            catch 
            {
            }

            // if no binding, succeed.
            if (mappingForForallVariable == null)
            {
                _logger.LogInfo($"Variable {_variable} contained no value: success!");
                yield return initialForallSolution;
                yield break;
            }

            // if bound to term, fail.
            if (mappingForForallVariable is TermBinding tb)
            {
                _logger.LogInfo($"Variable {_variable} was bound to term {tb.Term}: failure.");
                continue;
            }

            // now we know it is a prohibited values binding.
            var prohibitedValuesForVariable = (ProhibitedValuesBinding)mappingForForallVariable;

            // if no prohibited values(unconstrained), then succeed.
            if (prohibitedValuesForVariable.ProhibitedValues.Count == 0)
            {
                _logger.LogInfo($"Variable {_variable} is unconstrained: success!");
                yield return initialForallSolution;
                yield break;
            }

            // update goal with whatever we have found out about its vars during initial forall execution.
            var updatedGoal = initialForallSolution.ResultMapping.ApplySubstitution(_goalTerm);

            // get the "new version" of the variable:
            // forall(X, p(X)) would have X renamed during unification with database clause.
            var updatedVar = initialForallSolution.ResultMapping.Resolve(_variable, false);

            // construct new goals where variable in goalTerm is substituted by each prohibited value of variable.
            var constraintSubstitutedGoals = prohibitedValuesForVariable.ProhibitedValues
            .Select(prohibitedTerm => updatedGoal.Substitute
            (
                new Dictionary<Variable, ISimpleTerm>(new VariableComparer())
                {
                    {(Variable)((TermBinding)updatedVar).Term, prohibitedTerm }
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

            _logger.LogInfo($"Attempting to solve constraint-substituted goals {constraintSubstitutedGoals.ToList().ListToString()}");

            IEnumerable<GoalSolution> solutions = _solver.SolveGoals(initialSolvingState);

            foreach (GoalSolution solution in solutions ) 
            {
                var newSolution = new GoalSolution
                (
                    solution.ResultSet, 
                    solution.ResultMapping.SetItem(_variable, new ProhibitedValuesBinding()),
                    solution.Stack,
                    solution.NextInternalVariable
                );

                 yield return newSolution;
            }
        }
    }

 
}
