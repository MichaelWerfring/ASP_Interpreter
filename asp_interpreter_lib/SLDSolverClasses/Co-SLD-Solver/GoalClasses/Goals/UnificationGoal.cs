using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class UnificationGoal : ICoSLDGoal
{
    private readonly VariableMappingUpdater _variableMappingConcatenator = new VariableMappingUpdater();

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

        var unification = unificationMaybe.GetValueOrThrow();

        var newMappingEither = _variableMappingConcatenator
            .Update(_solutionState.CurrentMapping, unification);

        var newMapping = newMappingEither.GetRightOrThrow();

        yield return new GoalSolution(_solutionState.CurrentSet, newMapping, _solutionState.NextInternalVariableIndex);
    }
}
