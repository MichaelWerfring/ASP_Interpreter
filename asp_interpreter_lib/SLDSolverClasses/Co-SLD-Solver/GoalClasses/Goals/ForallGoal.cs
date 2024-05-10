using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class ForallGoal : ICoSLDGoal
{
    private readonly GoalSolver _solver;

    private readonly Variable _variable;

    private readonly ISimpleTerm _goalTerm;

    private readonly SolutionState _solutionState;

    public ForallGoal(GoalSolver solver, Variable variable, ISimpleTerm goalTerm, SolutionState solutionState)
    {
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(goalTerm, nameof(goalTerm));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        _solver = solver;
        _variable = variable;
        _goalTerm = goalTerm;
        _solutionState = solutionState;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        var initialState = new CoSldSolverState([_goalTerm], _solutionState);

        var successCaseState = new GoalSolution
        (
                new CoinductiveHypothesisSet(_solutionState.Set.Entries.Add(new CHSEntry(_goalTerm, true))),
                _solutionState.Mapping,
                _solutionState.NextInternalVariableIndex
        );

        foreach (var initialForallSolution in _solver.SolveGoals(initialState))
        {
            // get binding
            IVariableBinding? mappingForForallVariable;
            initialForallSolution.ResultMapping.Mapping.TryGetValue(_variable, out mappingForForallVariable);

            // if no binding, succeed.
            if (mappingForForallVariable == null)
            {
                yield return successCaseState;
                yield break;
            }

            // if bound to term, fail.
            if (mappingForForallVariable is TermBinding)
            {
                continue;
            }

            var prohibitedValuesForVariable = (ProhibitedValuesBinding) mappingForForallVariable;

            // if no prohibited values(unconstrained), then succeed.
            if (prohibitedValuesForVariable.ProhibitedValues.Count() == 0)
            {
                yield return successCaseState;
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

            // solve those goals, succeed if it has any solutions.
            var initialConstraintSolvingState = new 
                CoSldSolverState(constraintSubstitutedGoals, _solutionState);

            var solutions = _solver.SolveGoals(initialConstraintSolvingState);

            if (solutions.Any()) 
            {
                yield return successCaseState;
                yield break;
            }
        }
    }
}
