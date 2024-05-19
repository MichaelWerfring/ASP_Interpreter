using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;

public class CHSNoMatchOrConstrainmentResult : ICHSCheckingResult
{
    public CHSNoMatchOrConstrainmentResult(IEnumerable<VariableMapping> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        ConstrainmentResults = results;
    }

    public IEnumerable<VariableMapping> ConstrainmentResults { get; }

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
