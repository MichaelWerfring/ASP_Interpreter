namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;

public interface ICHSCheckingResult
{
    public void Accept(ICHSCheckingResultVisitor visitor);

    public TResult Accept<TResult>(ICHSCheckingResultVisitor<TResult> visitor);

    public void Accept<TArgs>(ICHSCheckingResultArgumentsVisitor<TArgs> visitor, TArgs args);

    public TResult Accept<TResult, TArgs>(ICHSCheckingResultArgumentsVisitor<TResult, TArgs> visitor, TArgs args);
}
