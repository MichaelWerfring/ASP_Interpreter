using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.Unification.Constructive.Target;

/// <summary>
/// A class that holds a target for a constructive (dis)unification algorithm. 
/// Basically two terms to unify, as well as their prohibited values lists.
/// The mapping must provide prohibited values lists for every variable in the left and right term.
/// It will not throw due to efficiency reasons of checking correctness,
/// but it will be problematic down the line.
/// </summary>
public class ConstructiveTarget
{
    public ConstructiveTarget(ISimpleTerm left, ISimpleTerm right, VariableMapping mapping)
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

    public VariableMapping Mapping { get; }
}
