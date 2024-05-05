using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using System.Collections.Immutable;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class ForallGoal : ICoSLDGoal
{
    private readonly IDatabase _database;

    private readonly Variable _variable;
    private readonly ISimpleTerm _goalTerm;

    private readonly FunctorTableRecord _functors;

    private readonly IImmutableList<ISimpleTerm> _nextGoals;
    private readonly SolutionState _solutionState;

    public ForallGoal
    (
        IDatabase database,
        Variable variable,
        ISimpleTerm goalTerm,
        FunctorTableRecord functors,
        IImmutableList<ISimpleTerm> nextGoals,
        SolutionState solutionState
    )
    {
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(goalTerm, nameof(goalTerm));
        ArgumentNullException.ThrowIfNull(nextGoals, nameof(nextGoals));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        _database = database;
        _variable = variable;
        _goalTerm = goalTerm;
        _functors = functors;
        _nextGoals = nextGoals;
        _solutionState = solutionState;
    }

    public IEnumerable<CoSldSolverState> TrySatisfy()
    {
        var goalSolver = new GoalSolver(new CoSLDGoalMapper(_functors), _database);
        var initialState = new CoSldSolverState([_goalTerm], _solutionState);
        var successCaseState = new CoSldSolverState
        (
            _nextGoals,
            new SolutionState
            (
                _solutionState.CurrentStack,
                new CoinductiveHypothesisSet(_solutionState.CurrentSet.Terms.Add(_goalTerm)),
                _solutionState.CurrentMapping,
                _solutionState.NextInternalVariableIndex
            )
        );

        foreach (var initialForallSolution in goalSolver.SolveGoals(initialState))
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
                CoSldSolverState(constraintSubstitutedGoals.ToImmutableList(), _solutionState);
            var solutions = goalSolver.SolveGoals(initialConstraintSolvingState);
            if (solutions.Count() > 0) 
            {
                yield return successCaseState;
                yield break;
            }
        }
    }
}
