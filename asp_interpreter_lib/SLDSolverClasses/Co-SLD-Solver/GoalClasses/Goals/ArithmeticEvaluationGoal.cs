using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

internal class ArithmeticEvaluationGoal : ICoSLDGoal
{
    private readonly ArithmeticEvaluator _evaluator;
    private readonly ISimpleTerm _left;
    private readonly ISimpleTerm _right;

    private readonly SolutionState _solutionState;

    public ArithmeticEvaluationGoal
    (
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        SolutionState solutionState
    )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(solutionState);

        _evaluator = evaluator;
        _left = left;
        _right = right;
        _solutionState = solutionState;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
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

            yield return new GoalSolution
            (_solutionState.CurrentSet, newMapping, _solutionState.NextInternalVariableIndex);
        }
        else if (_left is Integer leftInteger)
        {
            if (leftInteger.Value != rightEval)
            {
                yield break;
            }

            yield return new GoalSolution
            (_solutionState.CurrentSet, _solutionState.CurrentMapping, _solutionState.NextInternalVariableIndex);
        }
        else
        {
            yield break;
        }
    }
}
