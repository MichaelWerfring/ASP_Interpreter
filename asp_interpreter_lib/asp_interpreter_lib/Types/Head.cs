using System.Reflection.Metadata.Ecma335;

namespace asp_interpreter_lib.Types;

public class Head
{
    //Choice rules are not supported yet
    public Head(ClassicalLiteral literal)
    {
        Literal = literal;
    }
    
    public ClassicalLiteral Literal { get; private set; }
}