namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;

public interface ICHSCheckingResultVisitor
{
    public void Visit(CHSDeterministicFailureResult result);

    public void Visit(CHSDeterministicSuccessResult result);

    public void Visit(CHSNondeterministicSuccessResult result);
}

public interface ICHSCheckingResultVisitor<T>
{
    public T Visit(CHSDeterministicFailureResult result);

    public T Visit(CHSDeterministicSuccessResult result);

    public T Visit(CHSNondeterministicSuccessResult result);
}

public interface ICHSCheckingResultArgumentsVisitor<TArgs>
{
    public void Visit(CHSDeterministicFailureResult result, TArgs args);

    public void Visit(CHSDeterministicSuccessResult result, TArgs args);

    public void Visit(CHSNondeterministicSuccessResult result, TArgs args);
}

public interface ICHSCheckingResultArgumentsVisitor<TResult, TArgs>
{
    public TResult Visit(CHSDeterministicFailureResult result, TArgs args);

    public TResult Visit(CHSDeterministicSuccessResult result, TArgs args);

    public TResult Visit(CHSNondeterministicSuccessResult result, TArgs args);
}