using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions;

public class InternalTermCloner : IInternalTermVisitor<IInternalTerm>
{
    public IInternalTerm Clone(IInternalTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IInternalTerm Visit(Variable variableTerm)
    {
        ArgumentNullException.ThrowIfNull(variableTerm);

        return new Variable(variableTerm.Identifier);
    }

    public IInternalTerm Visit(Structure basicTerm)
    {
        ArgumentNullException.ThrowIfNull(basicTerm);

        var copiedChildren = new List<IInternalTerm>();
        foreach (var child in basicTerm.Children)
        {
            copiedChildren.Add(child.Accept(this));
        }

        return new Structure(basicTerm.Functor.ToString(), copiedChildren);
    }

    public IInternalTerm Visit(Integer integer)
    {
        ArgumentNullException.ThrowIfNull(integer);

        return new Integer(integer.Number);
    }
}
