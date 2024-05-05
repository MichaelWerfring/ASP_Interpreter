using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Util;
using System.Collections.Immutable;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

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

        return new Structure(term.Functor.GetCopy(), copiedChildren.ToImmutableList());
    }

    public ISimpleTerm Visit(Integer integer)
    {
        return new Integer(integer.Value);
    }
}
