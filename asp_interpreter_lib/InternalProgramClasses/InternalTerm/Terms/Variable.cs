using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;

public class Variable : IInternalTerm
{
    public Variable(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        Identifier = identifier;
    }

    public string Identifier { get; }

    public void Accept(IInternalTermVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(IInternalTermVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public TResult Accept<TResult, TArgs>(IInternalTermVisitor<TResult, TArgs> visitor, TArgs arguments)
    {
        return visitor.Visit(this, arguments);
    }

    public override string ToString()
    {
        return Identifier;
    }
}
