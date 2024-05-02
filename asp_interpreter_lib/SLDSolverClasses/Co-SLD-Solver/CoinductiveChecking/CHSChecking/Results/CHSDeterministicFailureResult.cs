namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;

public class CHSDeterministicFailureResult : ICHSCheckingResult
{
    public void Accept(ICHSCheckingResultVisitor visitor)
    {
        visitor.Visit(this);
    }

    public TResult Accept<TResult>(ICHSCheckingResultVisitor<TResult> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept<TArgs>(ICHSCheckingResultArgumentsVisitor<TArgs> visitor, TArgs args)
    {
        visitor.Visit(this, args);
    }

    public TResult Accept<TResult, TArgs>(ICHSCheckingResultArgumentsVisitor<TResult, TArgs> visitor, TArgs args)
    {
        return visitor.Visit(this, args);
    }
}
