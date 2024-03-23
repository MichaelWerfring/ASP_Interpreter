namespace asp_interpreter_lib.Types.Terms;

public class ParenthesizedTerm: Term
{
    public ParenthesizedTerm(Term term)
    {
        Term = term;
    }

    public Term Term { get; set; }
}