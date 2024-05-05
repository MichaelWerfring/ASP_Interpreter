namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;

public class CallstackDeterministicFailureResult : ICallstackCheckingResult
{
    public void Accept(ICallstackCheckingResultVisitor visitor)
    {
        visitor.Visit(this);
    }

    public TResult Accept<TResult>(ICallstackCheckingResultVisitor<TResult> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept<TArgs>(ICallstackCheckingResultArgumentVisitor<TArgs> visitor, TArgs args)
    {
        visitor.Visit(this, args);
    }

    public TResult Accept<TResult, TArgs>(ICallstackCheckingResultArgumentVisitor<TResult, TArgs> visitor, TArgs args)
    {
        return visitor.Visit(this, args);
    }
}
