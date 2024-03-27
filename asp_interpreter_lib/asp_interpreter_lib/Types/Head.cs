using System.Reflection.Metadata.Ecma335;

namespace asp_interpreter_lib.Types;

public class Head
{
    private ClassicalLiteral? _literal;

    public Head()
    {
        _literal = null;
    }
    
    public Head(ClassicalLiteral? literal)
    {
        Literal = literal;
    }

    public ClassicalLiteral? Literal
    {
        get => _literal;
        //Is only allowed from constructor
        private set => _literal = value ?? throw new ArgumentNullException(nameof(Literal));
    }

    public bool HasValue => _literal != null;

    public override string ToString()
    {
        return Literal?.ToString() ?? String.Empty;
    }
}