namespace asp_interpreter_lib.Types;

public class NafLiteral
{
    private ClassicalLiteral _literal;

    public NafLiteral(ClassicalLiteral literal, bool negated)
    {
        Literal = literal;
        Negated = negated;
    }

    //Negated in this context means negation as failure (NAF)
    public bool Negated { get; private set; }

    public ClassicalLiteral Literal
    {
        get => _literal;
        private set => _literal = value ?? throw new ArgumentNullException(nameof(Literal));
    }
}