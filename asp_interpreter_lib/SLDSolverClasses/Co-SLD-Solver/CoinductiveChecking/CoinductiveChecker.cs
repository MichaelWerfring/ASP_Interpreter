// <copyright file="CoinductiveChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling;

public class CoinductiveChecker : ICHSCheckingResultArgumentsVisitor<IEnumerable<CoinductiveCheckingResult>, (Structure, SolutionState)>
{
    private readonly CHSChecker _chsChecker;
    private readonly CallstackChecker _callstackChecker;
    private readonly CallstackResultToCoinductiveSuccesstypeConverter _converter;

    public CoinductiveChecker(CHSChecker chsChecker, CallstackChecker callstackChecker)
    {
        ArgumentNullException.ThrowIfNull(chsChecker, nameof(chsChecker));
        ArgumentNullException.ThrowIfNull(callstackChecker, nameof(callstackChecker));

        _chsChecker = chsChecker;
        _callstackChecker = callstackChecker;
        _converter = new();
    }

    /// <summary>
    /// Enumerates all the ways that the target can "survive" the coinductive checking process.
    /// </summary>
    public IEnumerable<CoinductiveCheckingResult> Check(Structure target, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(state, nameof(state));

        ICHSCheckingResult chsCheckingResult = _chsChecker.CheckCHS(target, state);

        IEnumerable<CoinductiveCheckingResult> results = chsCheckingResult.Accept(this, (target, state));

        foreach (var result in results)
        {
            yield return result;
        }
    }

    public IEnumerable<CoinductiveCheckingResult> Visit(CHSDeterministicFailureResult result, (Structure, SolutionState) checkingInput)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(checkingInput);

        yield break;
    }

    public IEnumerable<CoinductiveCheckingResult> Visit(CHSDeterministicSuccessResult result, (Structure, SolutionState) checkingInput)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(checkingInput);

        yield return new CoinductiveCheckingResult(checkingInput.Item1, checkingInput.Item2.Mapping, SuccessType.DeterministicSuccess);
    }

    public IEnumerable<CoinductiveCheckingResult> Visit(CHSNoMatchOrConstrainmentResult result, (Structure, SolutionState) checkingInput)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(checkingInput);

        foreach (VariableMapping constrainment in result.ConstrainmentResults)
        {
            Structure targetwithConstraintment = constrainment.ApplySubstitution(checkingInput.Item1);

            ICallstackCheckingResult callstackCheckingResult = _callstackChecker.CheckCallstack
                (targetwithConstraintment, constrainment, checkingInput.Item2.Callstack);

            IOption<SuccessType> resultTypeMaybe = _converter.Convert(callstackCheckingResult);

            if (!resultTypeMaybe.HasValue)
            {
                continue;
            }

            yield return new CoinductiveCheckingResult
            (
                targetwithConstraintment,
                constrainment,
                resultTypeMaybe.GetValueOrThrow()
            );
        }
    }
}
