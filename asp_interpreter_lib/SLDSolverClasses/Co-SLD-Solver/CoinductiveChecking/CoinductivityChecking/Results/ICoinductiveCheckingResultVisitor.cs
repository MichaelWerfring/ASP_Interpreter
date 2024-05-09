namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CoinductivityChecking.Results;

internal interface ICoinductiveCheckingResultVisitor
{
    public void Visit(CoinductiveDeterministicFailureResult result);

    public void Visit(CoinductiveDeterministicSuccessResult result);

    public void Visit(CoinductiveCallstackCheckingResult result);
}

internal interface ICoinductiveCheckingResultVisitor<T>
{
    public T Visit(CoinductiveDeterministicFailureResult result);

    public T Visit(CoinductiveDeterministicSuccessResult result);

    public T Visit(CoinductiveCallstackCheckingResult result);
}

internal interface ICoinductiveCheckingResultArgumentVisitor<TArgs>
{
    public void Visit(CoinductiveDeterministicFailureResult result, TArgs args);

    public void Visit(CoinductiveDeterministicSuccessResult result, TArgs args);

    public void Visit(CoinductiveCallstackCheckingResult result, TArgs args);
}

internal interface ICoinductiveCheckingResultArgumentVisitor<TResult, TArgs>
{
    public TResult Visit(CoinductiveDeterministicFailureResult result, TArgs args);

    public TResult Visit(CoinductiveDeterministicSuccessResult result, TArgs args);

    public TResult Visit(CoinductiveCallstackCheckingResult result, TArgs args);
}