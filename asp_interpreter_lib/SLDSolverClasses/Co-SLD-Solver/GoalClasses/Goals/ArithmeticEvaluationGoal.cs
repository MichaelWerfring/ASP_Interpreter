using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.ArithmeticEvaluation;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

internal class ArithmeticEvaluationGoal : ICoSLDGoal
{
    private readonly VariableMappingSubstituter _substituter = new VariableMappingSubstituter();

    private readonly ArithmeticEvaluator _evaluator;
    private readonly ISimpleTerm _left;
    private readonly ISimpleTerm _right;

    private readonly IImmutableList<ISimpleTerm> _nextGoals;
    private readonly SolutionState _solutionState;

    public ArithmeticEvaluationGoal
    (
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        IImmutableList<ISimpleTerm> nextGoals,
        SolutionState solutionState
        )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(nextGoals);
        ArgumentNullException.ThrowIfNull(solutionState);

        _evaluator = evaluator;
        _left = left;
        _right = right;
        _nextGoals = nextGoals;
        _solutionState = solutionState;
    }

    public IEnumerable<CoSldSolverState> TrySatisfy()
    {
        var rightEvalMaybe = _evaluator.Evaluate( _right );
        if( !rightEvalMaybe.HasValue )
        {
            yield break; 
        }

        var rightEval = rightEvalMaybe.GetValueOrThrow();

        if (_left is Variable leftVariable)
        {
            if
            (   
                ( (ProhibitedValuesBinding) _solutionState.CurrentMapping.Mapping[leftVariable] )
                .ProhibitedValues.Contains(new Integer(rightEval)) 
            )
            {
                yield break;
            }

            var newMapping = new VariableMapping
                (_solutionState.CurrentMapping.Mapping.SetItem(leftVariable, new TermBinding(new Integer(rightEval))));


            var substitutedGoals = _nextGoals.Select(x => _substituter.Substitute(x, newMapping));

            yield return new CoSldSolverState
            (
                substitutedGoals.ToImmutableList(),
                new SolutionState
                (_solutionState.CurrentStack, _solutionState.CurrentSet, newMapping, _solutionState.NextInternalVariableIndex)
            );
            yield break;
        }

        if (_left is Integer leftInteger)
        {
            if (leftInteger.Value != rightEval)
            {
                yield break;
            }

            yield return new CoSldSolverState(_nextGoals.ToImmutableList(), _solutionState);
            yield break;
        }

        yield break;
    }
}
