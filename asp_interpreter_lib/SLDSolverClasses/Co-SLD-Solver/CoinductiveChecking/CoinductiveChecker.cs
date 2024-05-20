using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;

public class CoinductiveChecker
{
    private readonly CHSChecker _chsChecker;

    private readonly CallstackChecker _callstackChecker;

    public CoinductiveChecker(CHSChecker chsChecker, CallstackChecker callstackChecker)
    {
        ArgumentNullException.ThrowIfNull(chsChecker, nameof(chsChecker));
        ArgumentNullException.ThrowIfNull(callstackChecker, nameof(callstackChecker));

        _chsChecker = chsChecker;
        _callstackChecker = callstackChecker;
    }

    /// <summary>
    /// Enumerates all the ways that the target can "survive" the coinductive checking process.
    /// </summary>
    public IEnumerable<CoinductiveCheckingResult> Check(Structure target, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(state, nameof(state));

        ICHSCheckingResult chsCheckingResult = _chsChecker.CheckCHS(target, state);

        if (chsCheckingResult is CHSDeterministicFailureResult)
        {
            yield break;
        }

        if (chsCheckingResult is CHSDeterministicSuccessResult)
        {
            yield return new CoinductiveCheckingResult(target, state.Mapping, SuccessType.DeterministicSuccess);
            yield break;
        }

        var chsConstraintmentResult = chsCheckingResult as CHSNoMatchOrConstrainmentResult
            ?? throw new InvalidDataException("Type hierarchy has changed: change this method to consider new type.");

        foreach (VariableMapping result in chsConstraintmentResult.ConstrainmentResults)
        {
            var targetwithConstraintment = result.ApplySubstitution(target);

            ICallstackCheckingResult callstackCheckingResult = _callstackChecker.CheckCallstack
                (targetwithConstraintment, result, state.Callstack);

            if (callstackCheckingResult is CallstackDeterministicFailureResult)
            {
                yield break;
            }

            IOption<SuccessType> resultTypeMaybe = ConvertToResultType(callstackCheckingResult);

            if (!resultTypeMaybe.HasValue)
            {
                yield break;
            }

            yield return new CoinductiveCheckingResult
            (
                targetwithConstraintment,
                result,
                resultTypeMaybe.GetValueOrThrow()
            );
        }
    }

    private IOption<SuccessType> ConvertToResultType(ICallstackCheckingResult result)
    {
        return result switch
        {
            CallstackDeterministicFailureResult => new None<SuccessType>(),
            CallstackDeterministicSuccessResult => new Some<SuccessType>(SuccessType.DeterministicSuccess),
            CallstackNondeterministicSuccessResult => new Some<SuccessType>(SuccessType.NonDeterministicSuccess),
            CallStackNoMatchResult => new Some<SuccessType>(SuccessType.NoMatch),
            _ => throw new InvalidDataException("Type hierarchy has changed: change this method to consider new type."),
        };
    }
}
