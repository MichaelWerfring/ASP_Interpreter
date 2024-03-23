using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types;

public class ClassicalLiteral
{
    public ClassicalLiteral(string identifier, bool negated, List<Term> terms)
    {
        Identifier = identifier;
        Terms = terms;
        Negated = negated;
    }

    public List<Term> Terms { get; private set; }
    
    //Negated in this context means classical negation 
    public bool Negated { get; set; }
    
    public string Identifier { get; set; }
}