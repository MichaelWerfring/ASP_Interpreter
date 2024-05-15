using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

internal class ArithmeticEvaluationGoal : ICoSLDGoal, ISimpleTermArgsVisitor<IOption<GoalSolution>, int>
{
    private SolverStateUpdater _updater = new();

    private readonly ArithmeticEvaluator _evaluator;

    private readonly ISimpleTerm _left;

    private readonly ISimpleTerm _right;

    private readonly SolutionState _state;

    private readonly IConstructiveUnificationAlgorithm _algorithm;

    public ArithmeticEvaluationGoal
    (
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        SolutionState state,
        IConstructiveUnificationAlgorithm algorithm
    )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(algorithm);

        _evaluator = evaluator;
        _left = left;
        _right = right;
        _state = state;
        _algorithm = algorithm;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        IOption<int> rightEvalMaybe = _evaluator.Evaluate( _right );

        int rightEval;
        try
        {
            rightEval = rightEvalMaybe.GetValueOrThrow();
        }
        catch
        {
            yield break;
        }

        var solutionMaybe = _left.Accept(this, rightEval);    
        if (!solutionMaybe.HasValue)
        {
            yield break;
        }

        yield return solutionMaybe.GetValueOrThrow();
    }

    public IOption<GoalSolution> Visit(Variable var, int integer)
    {
        var targetMaybe = ConstructiveTargetBuilder.Build(var, new Integer(integer), _state.Mapping);

        if(!targetMaybe.HasValue)
        {
            throw new ArgumentException("Mapping must not contain term value for left variable!");
        }

        var target = targetMaybe.GetValueOrThrow();

        IOption<VariableMapping> unificationMaybe = _algorithm.Unify(target);

        if (!unificationMaybe.HasValue)
        {
            return new None<GoalSolution>();
        }

        VariableMapping unification = unificationMaybe.GetValueOrThrow();

        CoinductiveHypothesisSet updatedCHS = _updater.UpdateCHS(_state.CHS, unification);

        CallStack updatedStack = _updater.UpdateCallstack(_state.Callstack, unification);

        return new Some<GoalSolution>
        (
            new GoalSolution
            (
                updatedCHS,
                unification,
                updatedStack,
                _state.NextInternalVariableIndex
            )
        );
    }

    public IOption<GoalSolution> Visit(Structure _, int __)
    {
        return new None<GoalSolution>();
    }

    public IOption<GoalSolution> Visit(Integer integer, int rightEval)
    {
        if (integer.Value != rightEval)
        {
            return new None<GoalSolution>();
        }
        else
        {
            return new Some<GoalSolution>
            (
                new GoalSolution
                (
                    _state.CHS,
                    _state.Mapping,
                    _state.Callstack, 
                    _state.NextInternalVariableIndex
                )
            );
        }
    }
}
