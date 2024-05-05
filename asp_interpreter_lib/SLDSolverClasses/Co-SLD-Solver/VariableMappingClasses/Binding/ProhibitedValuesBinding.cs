using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

public class ProhibitedValuesBinding : IVariableBinding
{
    public ProhibitedValuesBinding(IImmutableSet<ISimpleTerm> prohibitedValuesSet)
    {
        ArgumentNullException.ThrowIfNull(prohibitedValuesSet, nameof(prohibitedValuesSet));


        ProhibitedValues = prohibitedValuesSet;
    }

    public IImmutableSet<ISimpleTerm> ProhibitedValues { get; }

    // visitor
    public void Accept(IVariableBindingVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(IVariableBindingVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept<TArgs>(IVariableBindingArgumentVisitor<TArgs> visitor, TArgs arguments)
    {
        visitor.Visit(this, arguments);
    }

    public TResult Accept<TResult, TArgs>(IVariableBindingArgumentVisitor<TResult, TArgs> visitor, TArgs arguments)
    {
        return visitor.Visit(this, arguments);
    }

    public override string ToString()
    {
        return $"{{{ProhibitedValues.ToList().ListToString()}}}";
    }
}
