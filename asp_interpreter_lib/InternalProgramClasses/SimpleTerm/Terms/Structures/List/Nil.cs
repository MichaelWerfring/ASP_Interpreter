using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;

public class Nil : IStructure
{
    public void Accept(ISimpleTermVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(ISimpleTermVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept<TArgs>(ISimpleTermArgsVisitor<TArgs> visitor, TArgs arguments)
    {
        visitor.Visit(this, arguments);
    }

    public TResult Accept<TResult, TArgs>(ISimpleTermArgsVisitor<TResult, TArgs> visitor, TArgs arguments)
    {
        return visitor.Visit(this, arguments);
    }

    public override string ToString()
    {
        return $"[]";
    }
}
