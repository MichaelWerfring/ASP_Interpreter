using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class DisunificationGoal : ICoSLDGoal
{
    private readonly VariableMappingConcatenator _variableMappingConcatenator = new VariableMappingConcatenator();
    private readonly VariableMappingSubstituter _substitutor = new VariableMappingSubstituter();

    private readonly ConstructiveTarget _target;
    private readonly IConstructiveDisunificationAlgorithm _algorithm;

    private readonly IImmutableList<ISimpleTerm> _nextGoals;
    private readonly SolutionState _solutionState;

    public DisunificationGoal
    (
        ConstructiveTarget target,
        IConstructiveDisunificationAlgorithm algorithm,
        IImmutableList<ISimpleTerm> nextGoals,
        SolutionState solutionState
    )
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(nextGoals, nameof(nextGoals));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        _target = target;
        _algorithm = algorithm;
        _nextGoals = nextGoals;
        _solutionState = solutionState;
    }

    public IEnumerable<CoSldSolverState> TrySatisfy()
    {
        var disunificationMaybe = _algorithm.Disunify(_target);
        if (!disunificationMaybe.IsRight)
        {
            yield break;
        }

        var results = disunificationMaybe.GetRightOrThrow();

        foreach (var result in results)
        {
            var concatenationEither = _variableMappingConcatenator.Concatenate(_solutionState.CurrentMapping, result);

            var concatenation = concatenationEither.GetRightOrThrow();

            var substitutedGoals = _nextGoals.Select(x => _substitutor.Substitute(x, concatenation));

            var newSolutionState = new 
                SolutionState(_solutionState.CurrentStack, _solutionState.CurrentSet, concatenation, _solutionState.NextInternalVariableIndex);

            yield return new CoSldSolverState(substitutedGoals.ToImmutableList(), newSolutionState);
        }
    }
}
