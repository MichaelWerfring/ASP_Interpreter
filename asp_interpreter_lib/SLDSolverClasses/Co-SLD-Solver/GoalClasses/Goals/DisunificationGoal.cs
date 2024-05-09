using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class DisunificationGoal : ICoSLDGoal
{
    private readonly VariableMappingUpdater _variableMappingConcatenator = new VariableMappingUpdater();

    private readonly ConstructiveTarget _target;
    private readonly IConstructiveDisunificationAlgorithm _algorithm;
    private readonly SolutionState _solutionState;

    public DisunificationGoal
    (
        ConstructiveTarget target,
        IConstructiveDisunificationAlgorithm algorithm,
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
        var disunificationMaybe = _algorithm.Disunify(_target);
        if (!disunificationMaybe.IsRight)
        {
            yield break;
        }

        var results = disunificationMaybe.GetRightOrThrow();

        foreach (var result in results)
        {
            var concatenationEither = _variableMappingConcatenator.Update(_solutionState.CurrentMapping, result);

            var concatenation = concatenationEither.GetRightOrThrow();

            yield return new GoalSolution
                (_solutionState.CurrentSet, concatenation, _solutionState.NextInternalVariableIndex);
        }
    }
}
