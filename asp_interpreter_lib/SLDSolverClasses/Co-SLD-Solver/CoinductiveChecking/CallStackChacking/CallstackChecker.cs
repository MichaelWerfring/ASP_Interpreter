using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
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
    private ExactMatchChecker _checker = new ExactMatchChecker(new StandardConstructiveUnificationAlgorithm(false));

    private ConstructiveTargetBuilder _builder = new ConstructiveTargetBuilder();

    private IConstructiveUnificationAlgorithm _unificationAlgorithm = new StandardConstructiveUnificationAlgorithm(false);

    private FunctorTableRecord _functors;

    public CallstackChecker(FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        _functors = functors;
    }

    public ICallstackCheckingResult CheckCallstack(Structure termToCheck, VariableMapping currentMapping, CallStack callstack)
    {
        ArgumentNullException.ThrowIfNull(termToCheck, nameof(termToCheck));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));
        ArgumentNullException.ThrowIfNull(callstack, nameof(callstack));

        int numberOfNegations = 0;
        foreach(var term in callstack.TermStack)
        {
            if (IsNegatedTerm(term)) { numberOfNegations += 1; }

            var target = _builder.Build(termToCheck, term, currentMapping);

            if (numberOfNegations == 0)
            {
                if (_checker.AreExactMatch(target))
                {
                    return new CallstackDeterministicFailureResult();
                }
            }
            else if (numberOfNegations % 2 == 0)
            {
                if (_checker.AreExactMatch(target))
                {
                    return new CallstackDeterministicSuccessResult();
                }

                if(_unificationAlgorithm.Unify(target).HasValue)
                {
                    return new CallstackNondeterministicSuccessResult();
                }
            }
        }

        return new CallStackNoMatchResult();
    }

    private bool IsNegatedTerm(ISimpleTerm term)
    {
        if 
        (
            term is Structure termStruct
            && 
            termStruct.Functor == _functors.NegationAsFailure
            &&
            termStruct.Children.Count == 1
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
