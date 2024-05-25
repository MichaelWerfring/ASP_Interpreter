using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling.Either;
using System.Collections.Immutable;

namespace Asp_interpreter_lib.Unification.Constructive.Target;

/// <summary>
/// A class that holds a target for a constructive (dis)unification algorithm. 
/// Constructor should NEVER be called directly, always use Builder.
/// </summary>
public class ConstructiveTarget
{
    public ConstructiveTarget(ISimpleTerm left, ISimpleTerm right, ImmutableDictionary<Variable, ProhibitedValuesBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(mapping);

        Left = left;
        Right = right;
        Mapping = mapping;
    }

    public ISimpleTerm Left { get; }

    public ISimpleTerm Right { get; }

    public ImmutableDictionary<Variable, ProhibitedValuesBinding> Mapping { get; }

    public override string ToString()
    {
        return $"{{ {Left}, {Right}, [{Mapping.ToList().ListToString()}] }}";
    }
}
