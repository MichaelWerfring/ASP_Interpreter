using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;

public class ForAll : IStructure
{
    public ForAll(ISimpleTerm varTerm, ISimpleTerm goal)
    {
        ArgumentNullException.ThrowIfNull(varTerm);
        ArgumentNullException.ThrowIfNull(goal);

        VarTerm = varTerm;
        Goal = goal;
    }

    public ISimpleTerm VarTerm { get; }

    public ISimpleTerm Goal { get; }

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
        return $"forall({VarTerm}, {Goal})";
    }
}
