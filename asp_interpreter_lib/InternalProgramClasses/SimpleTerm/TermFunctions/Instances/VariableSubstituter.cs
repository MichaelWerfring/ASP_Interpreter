using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System.Collections.Immutable;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class VariableSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, Dictionary<Variable, ISimpleTerm>>
{
    public ISimpleTerm Substitute(ISimpleTerm term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);

        return term.Accept(this, mapping);
    }

    public ISimpleTerm Visit(Variable term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        ISimpleTerm? value;
        mapping.TryGetValue(term, out value);

        if (value == null)
        {
            return new Variable(term.Identifier);
        }

        return value.Clone();
    }

    public ISimpleTerm Visit(Structure term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        var newChildren = new ISimpleTerm[term.Children.Count()];

        for (int i = 0; i < newChildren.Length; i++)
        {
            newChildren[i] = term.Children.ElementAt(i).Accept(this, mapping);
        }

        return new Structure(term.Functor.ToString(), newChildren.ToImmutableList());
    }

    public ISimpleTerm Visit(Integer integer, Dictionary<Variable, ISimpleTerm> arguments)
    {
        return new Integer(integer.Value);
    }
}
