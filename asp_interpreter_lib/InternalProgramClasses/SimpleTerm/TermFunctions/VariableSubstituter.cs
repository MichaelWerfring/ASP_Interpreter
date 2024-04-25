using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class VariableSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, Dictionary<Variable, ISimpleTerm>>
{
    private SimpleTermCloner _cloner = new SimpleTermCloner();

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

        return _cloner.Clone(value);
    }

    public ISimpleTerm Visit(Structure term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        var newChildren = new ISimpleTerm[term.Children.Count()];

        for (int i = 0; i < newChildren.Length; i++)
        {
            newChildren[i] = term.Children.ElementAt(i).Accept(this, mapping);
        }

        return new Structure(term.Functor.ToString(), newChildren);
    }

    public ISimpleTerm Visit(Integer integer, Dictionary<Variable, ISimpleTerm> arguments)
    {
        return new Integer(integer.Value);
    }
}
