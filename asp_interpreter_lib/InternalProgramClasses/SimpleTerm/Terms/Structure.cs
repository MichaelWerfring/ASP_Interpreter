using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;
using asp_interpreter_lib.ListExtensions;
using System.Text;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

public class Structure : ISimpleTerm
{
    public Structure(string functor, IEnumerable<ISimpleTerm> children)
    {
        ArgumentException.ThrowIfNullOrEmpty(functor);
        ArgumentNullException.ThrowIfNull(children);

        Functor = functor;
        Children = children;
    }

    public string Functor { get; }

    public IEnumerable<ISimpleTerm> Children { get; }

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
        StringBuilder stringBuilder = new StringBuilder();

        if(Children.Count() == 0) { return Functor; }

        stringBuilder.Append(Functor);
        stringBuilder.Append('(');
        stringBuilder.Append(Children.ToList().ListToString());
        stringBuilder.Append(')');

        return stringBuilder.ToString();
    }
}
