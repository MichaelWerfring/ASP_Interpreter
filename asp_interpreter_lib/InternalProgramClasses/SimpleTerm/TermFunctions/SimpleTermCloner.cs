using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermCloner : ISimpleTermVisitor<ISimpleTerm>
{
    public ISimpleTerm Clone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public ISimpleTerm Visit(Variable variableTerm)
    {
        ArgumentNullException.ThrowIfNull(variableTerm);

        return new Variable(variableTerm.Identifier);
    }

    public ISimpleTerm Visit(Structure basicTerm)
    {
        ArgumentNullException.ThrowIfNull(basicTerm);

        var copiedChildren = new List<ISimpleTerm>();
        foreach (var child in basicTerm.Children)
        {
            copiedChildren.Add(child.Accept(this));
        }

        return new Structure(basicTerm.Functor.ToString(), copiedChildren);
    }
}
