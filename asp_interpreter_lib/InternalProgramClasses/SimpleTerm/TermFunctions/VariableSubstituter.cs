using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;

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

    public ISimpleTerm Visit(Variable variableTerm, Dictionary<Variable, ISimpleTerm> variables)
    {
        ArgumentNullException.ThrowIfNull(variableTerm);
        ArgumentNullException.ThrowIfNull(variables);

        ISimpleTerm? value;
        variables.TryGetValue(variableTerm, out value);

        if (value == null)
        {
            return _cloner.Clone(variableTerm);
        }

        return _cloner.Clone(value);
    }

    public ISimpleTerm Visit(Structure basicTerm, Dictionary<Variable, ISimpleTerm> variables)
    {
        ArgumentNullException.ThrowIfNull(basicTerm);
        ArgumentNullException.ThrowIfNull(variables);

        var newChildren = new ISimpleTerm[basicTerm.Children.Count()];

        for (int i = 0; i < newChildren.Length; i++)
        {
            newChildren[i] = basicTerm.Children.ElementAt(i).Accept(this, variables);
        }

        return new Structure(basicTerm.Functor.ToString(), newChildren);
    }
}
