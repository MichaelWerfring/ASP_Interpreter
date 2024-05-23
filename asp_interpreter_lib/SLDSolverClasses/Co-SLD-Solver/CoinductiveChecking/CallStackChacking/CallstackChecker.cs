using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Target.Builder;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Util.ErrorHandling.Either;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CallstackChecker
{
    private readonly ExactMatchChecker _checker;
    private readonly StandardConstructiveUnificationAlgorithm _unificationAlgorithm;
    private readonly FunctorTableRecord _functors;
    private readonly ILogger _logger;

    public CallstackChecker   
    (
        FunctorTableRecord functors,
        ExactMatchChecker checker,
        StandardConstructiveUnificationAlgorithm algo,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));
        ArgumentNullException.ThrowIfNull(checker, nameof(checker));
        ArgumentNullException.ThrowIfNull(algo, nameof(algo));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _checker = checker;
        _unificationAlgorithm = algo;
        _functors = functors;
        _logger = logger;
    }

    public ICallstackCheckingResult CheckCallstack(Structure termToCheck, VariableMapping currentMapping, CallStack callstack)
    {
        ArgumentNullException.ThrowIfNull(termToCheck, nameof(termToCheck));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));
        ArgumentNullException.ThrowIfNull(callstack, nameof(callstack));

        _logger.LogInfo($"Checking callstack for {termToCheck}");
        _logger.LogTrace($"Callstack is: {callstack}");
        _logger.LogTrace($"Current mapping is: {currentMapping}");

        int numberOfNegations = 0;

        foreach (ISimpleTerm term in callstack)
        {
            _logger.LogDebug($"Checking term in callstack: {term}");

            var targetEither = ConstructiveTargetBuilder.Build(termToCheck, term, currentMapping);

            if (!targetEither.IsRight)
            {
                _logger.LogError(
                    $"Could not build constructive target for {termToCheck} and {term}: {targetEither.GetLeftOrThrow().Message}");
                throw targetEither.GetLeftOrThrow();
            }

            ConstructiveTarget target = targetEither.GetRightOrThrow();
         
            if (numberOfNegations == 0)
            {
                if (_checker.AreExactMatch(target))
                {
                    _logger.LogInfo($"Found exact match with 0 negations: {term}. Det. failure!");
                    return new CallstackDeterministicFailureResult();
                }
            }
            else
            {
                if (_checker.AreExactMatch(target))
                {
                    _logger.LogInfo($"Found exact match with {numberOfNegations} negations: {term}. Det. success!");
                    return new CallstackDeterministicSuccessResult();
                }

                if (_unificationAlgorithm.Unify(target).HasValue)
                {
                    _logger.LogInfo($"Found regular match with {numberOfNegations} negations: {term}. Nondet. success!");

                    return new CallstackNondeterministicSuccessResult();
                }
            }

            if (term.IsNegated(_functors))
            {
                numberOfNegations += 1;
            }
        }

        _logger.LogInfo($"Found no match for {termToCheck}");

        return new CallStackNoMatchResult();
    }
}
