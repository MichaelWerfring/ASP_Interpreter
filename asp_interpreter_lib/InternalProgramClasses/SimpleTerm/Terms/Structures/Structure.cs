using System.Collections.Immutable;
using System.Text;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.Util;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

public class Structure : IStructure
{
    public Structure(string functor, IEnumerable<ISimpleTerm> children)
    {
        ArgumentException.ThrowIfNullOrEmpty(functor);
        ArgumentNullException.ThrowIfNull(children);

        Functor = functor;
        Children = children.ToImmutableList();
    }

    public string Functor { get; }

    public IImmutableList<ISimpleTerm> Children { get; }

    public void Accept(ISimpleTermVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    public T Accept<T>(ISimpleTermVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }

    public void Accept<TArgs>(ISimpleTermArgsVisitor<TArgs> visitor, TArgs arguments)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(arguments);

        visitor.Visit(this, arguments);
    }

    public TResult Accept<TResult, TArgs>(ISimpleTermArgsVisitor<TResult, TArgs> visitor, TArgs arguments)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(arguments);

        return visitor.Visit(this, arguments);
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.Append(Functor);

        if (Children.Count > 0)
        {
            stringBuilder.Append('(');
            stringBuilder.Append(Children.ToList().ListToString());
            stringBuilder.Append(')');
        }

        return stringBuilder.ToString();
    }
}
