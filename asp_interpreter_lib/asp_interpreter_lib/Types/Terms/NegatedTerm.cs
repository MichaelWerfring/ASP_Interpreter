namespace asp_interpreter_lib.Types.Terms;

public class NegatedTerm: Term
{
    public NegatedTerm(Term term)
    {
        Term = term;
    }

    public Term Term { get; set; }
}