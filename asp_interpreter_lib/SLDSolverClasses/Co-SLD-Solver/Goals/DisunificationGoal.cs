using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Unification.Constructive;
using asp_interpreter_lib.Unification.Constructive.Disunification;


namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class DisunificationGoal : ICoSLDGoal
{
    private VariableMappingConcatenator _variableMappingConcatenator = new VariableMappingConcatenator();
    private VariableMappingSubstituter _substitutor = new VariableMappingSubstituter();

    private ConstructiveTarget _target;

    private IConstructiveDisunificationAlgorithm _algorithm;

    private CallStack _callStack;
    private CoinductiveHypothesisSet _hypothesisSet;
    private VariableMapping _mapping;
    private IEnumerable<ISimpleTerm> _nextGoals;

    public DisunificationGoal
    (
        ConstructiveTarget target,
        IConstructiveDisunificationAlgorithm algorithm,
        CallStack currentStack,
        CoinductiveHypothesisSet currentSet,
        VariableMapping currentMapping,
        IEnumerable<ISimpleTerm> nextGoals
    )
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(currentStack, nameof(currentStack));
        ArgumentNullException.ThrowIfNull(currentSet, nameof(currentSet));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));
        ArgumentNullException.ThrowIfNull(nextGoals, nameof(nextGoals));

        _target = target;
        _algorithm = algorithm;
        _callStack = currentStack;
        _hypothesisSet = currentSet;
        _mapping = currentMapping;
        _nextGoals = nextGoals;
    }

    public IEnumerable<CoSldSolverState> TrySatisfy()
    {
        var disunificationMaybe = _algorithm.Disunify(_target);
        if (!disunificationMaybe.IsRight)
        {
            yield break;
        }

        var disunifications = disunificationMaybe.GetRightOrThrow();

        foreach (var disunifier in disunifications)
        {

        }

        //yield return new CoSldSolverState
        //    (_callStack, _hypothesisSet, newMapping, _nextGoals.Select(x => _substitutor.Substitute(x, newMapping)));
    }
}
