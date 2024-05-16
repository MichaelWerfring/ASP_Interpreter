using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class UnificationGoal : ICoSLDGoal
{
    private readonly SolverStateUpdater _updater = new();

    private readonly ConstructiveTarget _target;

    private readonly IConstructiveUnificationAlgorithm _algorithm;

    private readonly SolutionState _solutionState;

    public UnificationGoal
    (
        ConstructiveTarget target,
        IConstructiveUnificationAlgorithm algorithm,
        SolutionState solutionState
    )
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        _target = target;
        _algorithm = algorithm;
        _solutionState = solutionState;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        var unificationMaybe = _algorithm.Unify(_target);
        if (!unificationMaybe.HasValue)
        {
            yield break;
        }

        var unifyingMapping = unificationMaybe.GetValueOrThrow();

        var updatedMapping = _solutionState.Mapping.Update(unifyingMapping).GetValueOrThrow();

        var flattenedMapping = updatedMapping.Flatten();

        CoinductiveHypothesisSet updatedCHS = _updater.UpdateCHS(_solutionState.CHS, flattenedMapping);

        CallStack updatedCallstack = _updater.UpdateCallstack(_solutionState.Callstack, flattenedMapping);

        yield return new GoalSolution       
        (
            updatedCHS,
            flattenedMapping,
            updatedCallstack,
            _solutionState.NextInternalVariableIndex
        );
    }
}
