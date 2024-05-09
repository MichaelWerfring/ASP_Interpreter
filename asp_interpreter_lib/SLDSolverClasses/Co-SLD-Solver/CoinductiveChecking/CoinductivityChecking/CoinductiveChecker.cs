using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CoinductivityChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using System.Data;

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

    public ICoinductiveCheckingResult Check(Structure target, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(state, nameof(state));

        throw new NotImplementedException();

        //var chsCheckingResult = _chsChecker.CheckCHS(target, state);

        //if (chsCheckingResult is CHSDeterministicFailureResult)
        //{
        //    return new CoinductiveDeterministicFailureResult();
        //}

        //if (chsCheckingResult is CHSDeterministicSuccessResult)
        //{
        //    return new CoinductiveDeterministicSuccessResult();
        //}

        //var constraintmentResult = (CHSConstrainmentResult)chsCheckingResult;

        //var callstackCheckingResults = constraintmentResult.ConstrainmentResults.Select(constraintment =>
        //{
        //    ISimpleTerm newTarget = constraintment.ApplySubstitution(target);

        //    var checkingResult = _callstackChecker.CheckCallstack(target, constraintment, state.CurrentStack);

        //    if( checkingResult is CallstackDeterministicFailureResult)
        //    {
        //        return new CoinductiveDeterministicFailureResult();
        //    }

        //    if (checkingResult is CallstackDeterministicSuccessResult)
        //    {
        //        return new CoinductiveDeterministicSuccessResult();
        //    }

        //    return new CoinductiveDeterministicFailureResult()
        //});

        // return new CoinductiveCallstackCheckingResult(callstackCheckingResults);
    }
}
