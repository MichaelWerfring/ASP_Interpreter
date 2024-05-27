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

/// <summary>
/// A class for checking for coinductive success and failure.
/// </summary>
public class CoinductiveChecker : ICHSCheckingResultArgumentsVisitor<IEnumerable<CoinductiveCheckingResult>, (Structure Target, SolutionState State)>
{
    private readonly CHSChecker chsChecker;
    private readonly CallstackChecker callstackChecker;
    private readonly CallstackResultToCoinductiveSuccesstypeConverter converter;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoinductiveChecker"/> class.
    /// </summary>
    /// <param name="chsChecker">The chs checker to use.</param>
    /// <param name="callstackChecker">The callstack checker to use.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="chsChecker"/> is null.
    /// ..<paramref name="callstackChecker"/> is null.</exception>
    public CoinductiveChecker(CHSChecker chsChecker, CallstackChecker callstackChecker)
    {
        ArgumentNullException.ThrowIfNull(chsChecker, nameof(chsChecker));
        ArgumentNullException.ThrowIfNull(callstackChecker, nameof(callstackChecker));

        this.chsChecker = chsChecker;
        this.callstackChecker = callstackChecker;
        this.converter = new();
    }

    /// <summary>
    /// Enumerates all the ways that the target can "survive" the coinductive checking process.
    /// </summary>
    /// <param name="target">The target to check.</param>
    /// <param name="state">The current state of the solution.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="target"/> is null,
    /// ..<paramref name="state"/> is null.</exception>
    /// <returns>An enumeration of results, since the checking process may be nondeterministic.</returns>
    public IEnumerable<CoinductiveCheckingResult> Check(Structure target, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(state, nameof(state));

        ICHSCheckingResult chsCheckingResult = this.chsChecker.CheckCHS(target, state);

        IEnumerable<CoinductiveCheckingResult> results = chsCheckingResult.Accept(this, (target, state));

        foreach (var result in results)
        {
            yield return result;
        }
    }

    /// <summary>
    /// Visits a chs checking result.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="checkingInput">The input arguments: the target to check and the current state.</param>
    /// <returns>An enumeration of all results. In this case, nothing.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="result"/> is null,
    /// ..<paramref name="checkingInput"/> is null.</exception>
    public IEnumerable<CoinductiveCheckingResult> Visit(CHSDeterministicFailureResult result, (Structure Target, SolutionState State) checkingInput)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(checkingInput);

        yield break;
    }

    /// <summary>
    /// Visits a chs checking result.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="checkingInput">The input arguments: the target to check and the current state.</param>
    /// <returns>An enumeration of all results. In this case, deterministic success.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="result"/> is null,
    /// ..<paramref name="checkingInput"/> is null.</exception>
    public IEnumerable<CoinductiveCheckingResult> Visit(CHSDeterministicSuccessResult result, (Structure Target, SolutionState State) checkingInput)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(checkingInput);

        yield return new CoinductiveCheckingResult(checkingInput.Target, checkingInput.State.Mapping, SuccessType.DeterministicSuccess);
    }

    /// <summary>
    /// Visits a chs checking result.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="checkingInput">The input arguments: the target to check and the current state.</param>
    /// <returns>An enumeration of all results. In this case, all the ways the target can survive the callstack check.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="result"/> is null,
    /// ..<paramref name="checkingInput"/> is null.</exception>
    public IEnumerable<CoinductiveCheckingResult> Visit(CHSNoMatchOrConstrainmentResult result, (Structure Target, SolutionState State) checkingInput)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(checkingInput);

        foreach (VariableMapping constrainment in result.ConstrainmentResults)
        {
            Structure targetwithConstraintment = constrainment.ApplySubstitution(checkingInput.Target);

            ICallstackCheckingResult callstackCheckingResult = this.callstackChecker.CheckCallstack(
                targetwithConstraintment, constrainment, checkingInput.State.Callstack);

            IOption<SuccessType> resultTypeMaybe = this.converter.Convert(callstackCheckingResult);

            if (!resultTypeMaybe.HasValue)
            {
                continue;
            }

            yield return new CoinductiveCheckingResult(
                targetwithConstraintment,
                constrainment,
                resultTypeMaybe.GetValueOrThrow());
        }
    }
}