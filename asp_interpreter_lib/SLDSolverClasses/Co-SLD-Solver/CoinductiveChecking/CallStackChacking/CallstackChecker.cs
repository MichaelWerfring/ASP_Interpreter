using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CallstackChecker
{
    private ConstructiveTargetBuilder _builder = new ConstructiveTargetBuilder();

    private ExactMatchChecker _checker;

    private IConstructiveUnificationAlgorithm _unificationAlgorithm;

    private FunctorTableRecord _functors;

    public CallstackChecker(FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        _unificationAlgorithm = new StandardConstructiveUnificationAlgorithm(true);

        _checker = new ExactMatchChecker(_unificationAlgorithm);

        _functors = functors;
    }

    public ICallstackCheckingResult CheckCallstack(Structure termToCheck, VariableMapping currentMapping, CallStack callstack)
    {
        ArgumentNullException.ThrowIfNull(termToCheck, nameof(termToCheck));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));
        ArgumentNullException.ThrowIfNull(callstack, nameof(callstack));

        int numberOfNegations = 0;
        if (termToCheck.IsNegated(_functors))
        {
            numberOfNegations += 1;
        }

        foreach(var term in callstack.TermStack)
        {
            var target = _builder.Build(termToCheck, term, currentMapping);

            if (numberOfNegations == 0)
            {
                if (_checker.AreExactMatch(target))
                {
                    return new CallstackDeterministicFailureResult();
                }
            }
            else
            {
                if (_checker.AreExactMatch(target))
                {
                    return new CallstackDeterministicSuccessResult();
                }

                if (_unificationAlgorithm.Unify(target).HasValue)
                {
                    return new CallstackNondeterministicSuccessResult();
                }
            }

            if (term.IsNegated(_functors))
            {
                numberOfNegations += 1; 
            }
        }

        return new CallStackNoMatchResult();
    }
}
