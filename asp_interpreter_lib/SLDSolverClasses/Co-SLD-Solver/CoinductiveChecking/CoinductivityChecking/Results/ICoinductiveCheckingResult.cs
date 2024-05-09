using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CoinductivityChecking.Results;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;

internal interface ICoinductiveCheckingResult
{
    public void Accept(ICoinductiveCheckingResultVisitor visitor);

    public T Accept<T>(ICoinductiveCheckingResultVisitor<T> visitor);

    public void Accept<TArgs>(ICoinductiveCheckingResultArgumentVisitor<TArgs> visitor, TArgs args);

    public TResult Accept<TResult, TArgs>(ICoinductiveCheckingResultArgumentVisitor<TResult, TArgs> visitor, TArgs args);
}