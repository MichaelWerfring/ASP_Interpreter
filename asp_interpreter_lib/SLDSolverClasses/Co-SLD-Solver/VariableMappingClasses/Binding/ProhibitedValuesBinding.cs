using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

public class ProhibitedValuesBinding : IVariableBinding
{
    public ProhibitedValuesBinding(HashSet<ISimpleTerm> prohibitedValuesSet)
    {
        ArgumentNullException.ThrowIfNull(prohibitedValuesSet, nameof(prohibitedValuesSet));
        if (prohibitedValuesSet.Comparer is not SimpleTermComparer)
        {
            throw new ArgumentException($"Comparer must be of type {typeof(SimpleTermComparer)}");
        }

        ProhibitedValues = prohibitedValuesSet;
    }

    public HashSet<ISimpleTerm> ProhibitedValues { get; }

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
