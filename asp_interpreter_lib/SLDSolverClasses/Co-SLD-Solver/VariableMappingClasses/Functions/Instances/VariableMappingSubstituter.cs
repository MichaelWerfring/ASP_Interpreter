using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;

public class VariableMappingSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, VariableMapping>
{
    /// <summary>
    /// Substitutes all variables in the term by their value in mapping, in case they have a termbinding.
    /// </summary>
    /// <param name="term"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public ISimpleTerm Substitute(ISimpleTerm term, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);
            
        return term.Accept(this, mapping);
    }

    public ISimpleTerm Visit(Variable variableTerm, VariableMapping map)
    {
        if (map.TryGetValue(variableTerm, out IVariableBinding? value) && value is TermBinding tb)
        {
            return tb.Term;
        }
        else
        {
            return variableTerm;
        }
    }

    public ISimpleTerm Visit(Structure basicTerm, VariableMapping map)
    {
        var newChildren = new ISimpleTerm[basicTerm.Children.Count];

        Parallel.For(0, newChildren.Length, index =>
        {
            newChildren[index] = basicTerm.Children.ElementAt(index).Accept(this, map);
        });

        return new Structure(basicTerm.Functor, newChildren);
    }

    public ISimpleTerm Visit(Integer integer, VariableMapping map)
    {
        return integer;
    }
}
