namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CallStackChacking.Results;


public interface ICallstackCheckingResultVisitor
{
    public void Visit(CallstackDeterministicFailureResult result);

    public void Visit(CallstackDeterministicSuccessResult result);

    public void Visit(CallstackNondeterministicSuccessResult result);
    void Visit(CallStackNoMatchResult callStackNoMatchResult);
}

public interface ICallstackCheckingResultVisitor<T>
{
    public T Visit(CallstackDeterministicFailureResult result);

    public T Visit(CallstackDeterministicSuccessResult result);

    public T Visit(CallstackNondeterministicSuccessResult result);

    public T Visit(CallStackNoMatchResult callStackNoMatchResult);
}

public interface ICallstackCheckingResultArgumentVisitor<TArgs>
{
    public void Visit(CallstackDeterministicFailureResult result, TArgs args);

    public void Visit(CallstackDeterministicSuccessResult result, TArgs args);

    public void Visit(CallstackNondeterministicSuccessResult result, TArgs args);

    public void Visit(CallStackNoMatchResult result, TArgs args);
}

public interface ICallstackCheckingResultArgumentVisitor<TResult, TArgs>
{
    public TResult Visit(CallstackDeterministicFailureResult result, TArgs args);

    public TResult Visit(CallstackDeterministicSuccessResult result, TArgs args);

    public TResult Visit(CallstackNondeterministicSuccessResult result, TArgs args);

    public TResult Visit(CallStackNoMatchResult result, TArgs args);
}
