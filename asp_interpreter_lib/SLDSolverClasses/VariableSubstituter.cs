using asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.SLDSolverClasses;

public class VariableSubstituter : IInternalTermVisitor<IInternalTerm, Dictionary<Variable, IInternalTerm>>
{
    private InternalTermCloner _cloner = new InternalTermCloner();

    public IInternalTerm Substitute(IInternalTerm term, Dictionary<Variable, IInternalTerm> mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);

        return term.Accept(this, mapping);
    }

    public IInternalTerm Visit(Variable variableTerm, Dictionary<Variable, IInternalTerm> variables)
    {
        ArgumentNullException.ThrowIfNull(variableTerm);
        ArgumentNullException.ThrowIfNull(variables);

        IInternalTerm? value;
        variables.TryGetValue(variableTerm, out value);

        if (value == null)
        {
            return _cloner.Clone(variableTerm);
        }

        return _cloner.Clone(value);
    }

    public IInternalTerm Visit(Structure basicTerm, Dictionary<Variable, IInternalTerm> variables)
    {
        ArgumentNullException.ThrowIfNull(basicTerm);
        ArgumentNullException.ThrowIfNull(variables);

        var newChildren = new IInternalTerm[basicTerm.Children.Count()];

        for (int i = 0; i < newChildren.Length; i++)
        {
            newChildren[i] = basicTerm.Children.ElementAt(i).Accept(this, variables);
        }

        return new Structure(basicTerm.Functor.ToString(), newChildren);
    }

    public IInternalTerm Visit(Integer integer, Dictionary<Variable, IInternalTerm> variables)
    {
        ArgumentNullException.ThrowIfNull(integer);
        ArgumentNullException.ThrowIfNull(variables);

        return _cloner.Clone(integer);
    }
}
