using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking;

internal class CallstackResultToCoinductiveSuccesstypeConverter : ICallstackCheckingResultVisitor<IOption<SuccessType>>
{
    public IOption<SuccessType> Convert(ICallstackCheckingResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.Accept(this);
    }

    public IOption<SuccessType> Visit(CallstackDeterministicFailureResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new None<SuccessType>();
    }

    public IOption<SuccessType> Visit(CallstackDeterministicSuccessResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new Some<SuccessType>(SuccessType.DeterministicSuccess);
    }

    public IOption<SuccessType> Visit(CallstackNondeterministicSuccessResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new Some<SuccessType>(SuccessType.NonDeterministicSuccess);
    }

    public IOption<SuccessType> Visit(CallStackNoMatchResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new Some<SuccessType>(SuccessType.NoMatch);
    }
}
