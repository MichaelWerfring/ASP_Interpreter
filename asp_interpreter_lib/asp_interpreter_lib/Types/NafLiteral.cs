namespace asp_interpreter_lib.Types;

public class NafLiteral
{
    public NafLiteral(ClassicalLiteral literal, bool negated)
    {
        Literal = literal;
        Negated = negated;
    }

    //Negated in this context means negation as failure (NAF)
    public bool Negated { get; set; }
    
    public ClassicalLiteral Literal { get; set; }
}