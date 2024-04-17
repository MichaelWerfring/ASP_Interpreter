using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermCloner : ISimpleTermVisitor<ISimpleTerm>
{
    public ISimpleTerm Clone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public ISimpleTerm Visit(Variable term)
    {
        return new Variable(term.Identifier);
    }

    public ISimpleTerm Visit(Structure term)
    {
        var copiedChildren = new List<ISimpleTerm>();
        foreach (var child in term.Children)
        {
            copiedChildren.Add(child.Accept(this));
        }

        return new Structure(term.Functor.GetCopy(), copiedChildren, term.IsNegated);
    }

    public ISimpleTerm Visit(Integer integer)
    {
        return new Integer(integer.Value);
    }
}
