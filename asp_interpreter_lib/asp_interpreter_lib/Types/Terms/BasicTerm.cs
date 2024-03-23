namespace asp_interpreter_lib.Types.Terms;

public class BasicTerm: Term
{
    public BasicTerm(string identifier, List<Term> terms)
    {
        Identifier = identifier;
        Terms = terms;
    }

    public string Identifier { get; set; }

    public List<Term> Terms { get; set; }
}