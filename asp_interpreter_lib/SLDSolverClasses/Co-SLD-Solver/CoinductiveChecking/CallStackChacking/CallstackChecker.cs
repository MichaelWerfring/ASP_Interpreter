// <copyright file="CallstackChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for checking a term against a callstack.
/// </summary>
public class CallstackChecker
{
    private readonly ExactMatchChecker checker;
    private readonly StandardConstructiveUnificationAlgorithm unificationAlgorithm;
    private readonly FunctorTableRecord functors;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallstackChecker"/> class.
    /// </summary>
    /// <param name="functors">The functor table to identify negations with.</param>
    /// <param name="checker">The exact match checker.</param>
    /// <param name="algo">The algorithm to check for unification.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..functors is null.
    /// ..checker is null.
    /// ..algo is null.
    /// ..logger is null.</exception>
    public CallstackChecker(
        FunctorTableRecord functors,
        ExactMatchChecker checker,
        StandardConstructiveUnificationAlgorithm algo,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));
        ArgumentNullException.ThrowIfNull(checker, nameof(checker));
        ArgumentNullException.ThrowIfNull(algo, nameof(algo));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        this.checker = checker;
        this.unificationAlgorithm = algo;
        this.functors = functors;
        this.logger = logger;
    }

    /// <summary>
    /// Checks a a term with a mapping. for matches in a callstack.
    /// </summary>
    /// <param name="termToCheck">The term to check.</param>
    /// <param name="currentMapping">The mapping for the term.</param>
    /// <param name="callstack">The callstack to check against.</param>
    /// <returns>A callstack checking result.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..termToCheck is null,
    /// ..currentMapping is null,
    /// ..callstack is null.</exception>
    public ICallstackCheckingResult CheckCallstack(Structure termToCheck, VariableMapping currentMapping, CallStack callstack)
    {
        ArgumentNullException.ThrowIfNull(termToCheck, nameof(termToCheck));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));
        ArgumentNullException.ThrowIfNull(callstack, nameof(callstack));

        this.logger.LogInfo($"Checking callstack for {termToCheck}");
        this.logger.LogTrace($"Callstack is: {callstack}");
        this.logger.LogTrace($"Current mapping is: {currentMapping}");

        int numberOfNegations = 0;

        foreach (Structure term in callstack)
        {
            this.logger.LogDebug($"Checking term in callstack: {term}");

            var targetEither = ConstructiveTargetBuilder.Build(termToCheck, term, currentMapping);

            if (!targetEither.IsRight)
            {
                throw targetEither.GetLeftOrThrow();
            }

            ConstructiveTarget target = targetEither.GetRightOrThrow();

            if (numberOfNegations == 0)
            {
                if (this.checker.AreExactMatch(target))
                {
                    return new CallstackDeterministicFailureResult();
                }
            }
            else
            {
                if (this.checker.AreExactMatch(target))
                {
                    return new CallstackDeterministicSuccessResult();
                }

                if (this.unificationAlgorithm.Unify(target).HasValue)
                {
                    return new CallstackNondeterministicSuccessResult();
                }
            }

            if (term.IsNegated(this.functors))
            {
                numberOfNegations += 1;
            }
        }

        return new CallStackNoMatchResult();
    }
}