using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class VariableSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, IDictionary<Variable, ISimpleTerm>>
{
    public ISimpleTerm Substitute(ISimpleTerm term, IDictionary<Variable, ISimpleTerm> mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);

        return term.Accept(this, mapping);
    }

    public ISimpleTerm Visit(Structure term, IDictionary<Variable, ISimpleTerm> mapping)
    {
        var newChildren = new ISimpleTerm[term.Children.Count()];

        Parallel.For(0, newChildren.Length, index =>
        {
            newChildren[index] = term.Children.ElementAt(index).Accept(this, mapping);
        });

        return new Structure(term.Functor, newChildren);
    }

    public ISimpleTerm Visit(Variable variable, IDictionary<Variable, ISimpleTerm> mapping)
    {
        ISimpleTerm? value;
        mapping.TryGetValue(variable, out value);

        if (value == null)
        {
            return variable;
        }

        return value;
    }

    public ISimpleTerm Visit(Integer integer, IDictionary<Variable, ISimpleTerm> arguments)
    {
        return integer;
    }
}
