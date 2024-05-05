using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class UnificationGoal : ICoSLDGoal
{
    private readonly VariableMappingConcatenator _variableMappingConcatenator = new VariableMappingConcatenator();
    private readonly VariableMappingSubstituter _substitutor = new VariableMappingSubstituter();

    private readonly ConstructiveTarget _target;
    private readonly IConstructiveUnificationAlgorithm _algorithm;

    private readonly IImmutableList<ISimpleTerm> _nextGoals;

    private readonly SolutionState _solutionState;

    public UnificationGoal
    (
        ConstructiveTarget target,
        IConstructiveUnificationAlgorithm algorithm,
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
        var unificationMaybe = _algorithm.Unify(_target);
        if (!unificationMaybe.HasValue)
        {
            yield break;
        }

        var unification = unificationMaybe.GetValueOrThrow();

        var newMappingEither = _variableMappingConcatenator.Concatenate(_solutionState.CurrentMapping, unification);
        VariableMapping newMapping;
        try
        {
            newMapping = newMappingEither.GetRightOrThrow();
        }
        catch
        {
            throw;
        }

        var substitutedGoals = _nextGoals.Select(x => _substitutor.Substitute(x, newMapping));

        var newSolutionState = new 
            SolutionState(_solutionState.CurrentStack, _solutionState.CurrentSet, newMapping, _solutionState.NextInternalVariableIndex);

        yield return new CoSldSolverState(substitutedGoals.ToImmutableList(), newSolutionState);
    }
}
