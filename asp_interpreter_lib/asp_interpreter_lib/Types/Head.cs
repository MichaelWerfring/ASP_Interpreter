using System.Reflection.Metadata.Ecma335;

namespace asp_interpreter_lib.Types;

public class Head
{
    //Choice rules are not supported yet
    public Head(List<ClassicalLiteral> literals)
    {
        if (literals.Count < 1)
        {
            throw new ArgumentException("Head must contain at least one literal");
        }
        
        Literals = literals;
    }
    
    public ClassicalLiteral Literal => Literals[0];

    public List<ClassicalLiteral> Literals { get; private set; }
}