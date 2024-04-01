using asp_interpreter_lib.SimplifiedTerm.Visitor;

namespace asp_interpreter_lib.SimplifiedTerm;

public class VariableTerm : ISimplifiedTerm
{
    public VariableTerm(string identifier)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(identifier);
        Identifier = identifier;
        Children = new List<ISimplifiedTerm>();
    }

    public string Identifier { get; }

    public IEnumerable<ISimplifiedTerm> Children { get; }

    public void Accept(ISimplifiedTermVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }

    public T Accept<T>(ISimplifiedTermVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return Identifier;
    }
}
