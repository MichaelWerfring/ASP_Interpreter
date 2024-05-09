using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CoinductivityChecking.Results;

internal class CoinductiveCallstackCheckingResult : ICoinductiveCheckingResult
{
    public CoinductiveCallstackCheckingResult(IEnumerable<ICallstackCheckingResult> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        CallstackCheckingResults = results;
    }

    public IEnumerable<ICallstackCheckingResult> CallstackCheckingResults { get; }

    public void Accept(ICoinductiveCheckingResultVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(ICoinductiveCheckingResultVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept<TArgs>(ICoinductiveCheckingResultArgumentVisitor<TArgs> visitor, TArgs args)
    {
        visitor.Visit(this, args);
    }

    public TResult Accept<TResult, TArgs>(ICoinductiveCheckingResultArgumentVisitor<TResult, TArgs> visitor, TArgs args)
    {
        return visitor.Visit(this, args);
    }
}
