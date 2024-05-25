using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class VariableSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, IDictionary<Variable, ISimpleTerm>>
{
    public ISimpleTerm Substitute(ISimpleTerm term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return term.Accept(this, map);
    }

    public Structure SubsituteStructure(Structure term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        var newChildren = new ISimpleTerm[term.Children.Count];

        Parallel.For(0, newChildren.Length, index =>
        {
            newChildren[index] = term.Children.ElementAt(index).Accept(this, map);
        });

        return new Structure(term.Functor, newChildren);
    }

    public ISimpleTerm Visit(Structure term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return SubsituteStructure(term, map);
    }

    public ISimpleTerm Visit(Variable term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        ISimpleTerm? value;
        map.TryGetValue(term, out value);

        if (value == null)
        {
            return term;
        }

        return value;
    }

    public ISimpleTerm Visit(Integer term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return term;
    }
}
