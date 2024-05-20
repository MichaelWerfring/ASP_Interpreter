using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Target.Builder;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;

public class CHSChecker
{
    private readonly ExactMatchChecker _checker;

    private readonly StandardConstructiveUnificationAlgorithm _algorithm;

    private readonly FunctorTableRecord _functors;

    private readonly GoalSolver _goalSolver;

    private readonly ILogger _logger;

    public CHSChecker
    (
        ExactMatchChecker checker,
        StandardConstructiveUnificationAlgorithm algo,
        FunctorTableRecord record,
        GoalSolver solver,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(checker);
        ArgumentNullException.ThrowIfNull(algo);
        ArgumentNullException.ThrowIfNull(record);
        ArgumentNullException.ThrowIfNull(solver);
        ArgumentNullException.ThrowIfNull(algo);

        _checker = checker;
        _algorithm = algo;
        _functors = record;
        _goalSolver = solver;
        _logger = logger;
    }

    public ICHSCheckingResult CheckCHS(Structure termToCheck, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(termToCheck, nameof(termToCheck));
        ArgumentNullException.ThrowIfNull(state);

        _logger.LogInfo($"Checking CHS for {termToCheck}");
        _logger.LogDebug($"CHS is: {state.CHS}");
        _logger.LogTrace($"Current mapping is: {state.Mapping}");

        // construct negatedTerm
        Structure negation = termToCheck.NegateTerm(_functors);

        // check for exact matches for negation of term
        _logger.LogInfo($"Checking for exact matches for negation of term..");
        if (HasExactMatch(negation, state.CHS, state.Mapping, false))
        {
            _logger.LogInfo($"Found exact match for negation. Deterministic failure.");
            return new CHSDeterministicFailureResult();
        }
        _logger.LogInfo($"No exact matches for negation found!");

        // check for exact matches for term
        _logger.LogInfo($"Checking for exact matches for term..");
        if (HasExactMatch(termToCheck, state.CHS, state.Mapping, true))
        {
            _logger.LogInfo($"Found exact match for term. Deterministic success.");

            return new CHSDeterministicSuccessResult();
        }
        _logger.LogInfo($"No exact matches for term found!");

        // get all terms that unify with the negation of input entry.
        IEnumerable<Structure> termsUnifyingWithNegation = GetUnifyingTerms(negation, state.CHS, state.Mapping);
        _logger.LogDebug($"Terms unifying with negation of input term: {termsUnifyingWithNegation.ToList().ListToString()}");

        // construct disunification goals
        IEnumerable<Structure> disunificationGoals = termsUnifyingWithNegation.AsParallel()
            .Select(term => new Structure(_functors.Disunification, [termToCheck, term]));

        // from that, construct initial state for solver.
        var newSolverState = new CoSldSolverState
        (
            disunificationGoals,
            new SolutionState(state.Callstack, state.CHS, state.Mapping, state.NextInternalVariableIndex)
        );

        if (termsUnifyingWithNegation.Any())
        {
            _logger.LogInfo($"Found unifying negations: {termsUnifyingWithNegation.ToList().ListToString()}");
            _logger.LogInfo($"Attempting to disunify {termToCheck} with them");
        }
        else
        {
            _logger.LogInfo($"No negations for term {termToCheck}. No match.");
        }

        // return all the ways that all these disunifications can be solved.
        return new CHSNoMatchOrConstrainmentResult
            (_goalSolver.SolveGoals(newSolverState).Select(x => x.ResultMapping));
    }

    private bool HasExactMatch(ISimpleTerm term, CoinductiveHypothesisSet set, VariableMapping mapping, bool mustHaveSucceeded)
    {
        try
        {
            if (set.AsParallel().Any(entry =>
            {
                if (mustHaveSucceeded && !entry.HasSucceded)
                {
                    return false;
                }

                ConstructiveTarget target;
                try
                {
                    target = ConstructiveTargetBuilder.Build(term, entry.Term, mapping).GetRightOrThrow();
                }
                catch
                {
                    _logger.LogError($"Could not build constructive target for {term} and {entry.Term}");
                    throw new Exception();
                }

                return _checker.AreExactMatch(target);
            }))
            {
                return true;
            }
        }
        catch
        {
            throw;
        }
        
        return false;
    }

    private ParallelQuery<Structure> GetUnifyingTerms(Structure term, CoinductiveHypothesisSet set, VariableMapping mapping)
    {
       return set.AsParallel().Where
       (
           entry =>
           {
               var target = ConstructiveTargetBuilder.Build(term, entry.Term, mapping).GetRightOrThrow();

               return _algorithm.Unify(target).HasValue;
           }
       )
       .Select(entry => entry.Term);
    }
}
