using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;

internal class CoinductiveChecker
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

        CHSConstrainmentResult chsConstraintmentResult = (CHSConstrainmentResult)chsCheckingResult;

        foreach (VariableMapping constraintment in chsConstraintmentResult.ConstrainmentResults)
        {
            Structure targetwithConstraintment = (Structure)constraintment.ApplySubstitution(target);

            ICallstackCheckingResult callstackCheckingResult = _callstackChecker.CheckCallstack
                (targetwithConstraintment, constraintment, state.Callstack);

            if (callstackCheckingResult is CallstackDeterministicFailureResult)
            {
                yield break;
            }

            if (callstackCheckingResult is CallstackDeterministicSuccessResult)
            {
                yield return new CoinductiveCheckingResult
                (
                    targetwithConstraintment,
                    constraintment,
                    SuccessType.DeterministicSuccess
                );
                yield break;
            }

            if (callstackCheckingResult is CallstackNondeterministicSuccessResult)
            {
                yield return new CoinductiveCheckingResult
                (
                    targetwithConstraintment,
                    constraintment,
                    SuccessType.NonDeterministicSuccess
                );
                yield break;
            }

            yield return new CoinductiveCheckingResult
            (
                targetwithConstraintment,
                constraintment,
                SuccessType.NoMatch
            );
        }
    }
}
