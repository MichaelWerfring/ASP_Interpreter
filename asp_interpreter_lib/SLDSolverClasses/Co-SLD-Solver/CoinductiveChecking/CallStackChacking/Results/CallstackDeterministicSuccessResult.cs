namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;

public class CallstackDeterministicSuccessResult : ICallstackCheckingResult
{
    public void Accept(ICallstackCheckingResultVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    public TResult Accept<TResult>(ICallstackCheckingResultVisitor<TResult> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }

    public void Accept<TArgs>(ICallstackCheckingResultArgumentVisitor<TArgs> visitor, TArgs args)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(args);

        visitor.Visit(this, args);
    }

    public TResult Accept<TResult, TArgs>(ICallstackCheckingResultArgumentVisitor<TResult, TArgs> visitor, TArgs args)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(args);

        return visitor.Visit(this, args);
    }
}
