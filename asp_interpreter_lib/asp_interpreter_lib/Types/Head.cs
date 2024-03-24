using System.Reflection.Metadata.Ecma335;

namespace asp_interpreter_lib.Types;

public class Head
{
    private ClassicalLiteral _literal;

    //Choice rules are not supported yet
    public Head(ClassicalLiteral literal)
    {
        Literal = literal;
    }

    public ClassicalLiteral Literal
    {
        get => _literal;
        private set => _literal = value ?? throw new ArgumentNullException(nameof(Literal));
    }
}