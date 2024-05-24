namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;

public interface ICallstackCheckingResult
{
    public void Accept(ICallstackCheckingResultVisitor visitor);

    public TResult Accept<TResult>(ICallstackCheckingResultVisitor<TResult> visitor);

    public void Accept<TArgs>(ICallstackCheckingResultArgumentVisitor<TArgs> visitor, TArgs args);

    public TResult Accept<TResult, TArgs>(ICallstackCheckingResultArgumentVisitor<TResult, TArgs> visitor, TArgs args);
}