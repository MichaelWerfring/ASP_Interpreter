using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using System.Collections.Immutable;

namespace asp_interpreter_lib.Unification.Constructive.Target;

/// <summary>
/// A class that holds a target for a constructive (dis)unification algorithm. 
/// Basically two terms to unify, as well as their prohibited values lists.
/// The mapping must provide prohibited values for every variable in the left and right term (and not more).
/// </summary>
public class ConstructiveTarget
{
    public ConstructiveTarget(ISimpleTerm left, ISimpleTerm right, IImmutableDictionary<Variable, ProhibitedValuesBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(mapping);

        Left = left;
        Right = right;

        // check for correctness of input mapping:
        // construct set of variables of both terms
        var variableSet = left.ToList()
                            .Union(right.ToList())
                            .Where(x => x is Variable)
                            .Select(x => (Variable)x)
                            .ToHashSet(new VariableComparer());

        // if any of the variables are not in the dictionary, then fail.
        if (variableSet.Any(var => mapping[var] == null))
        {
            throw new ArgumentException
                ($"Must contain prohibited value list for each variable in" +
                $" {nameof(left)} and {nameof(right)}", nameof(mapping));
        }

        // if mapping contains other variables, then fail.
        if (variableSet.Count != mapping.Count)
        {
            throw new ArgumentException
                ($"Must contain only variables for the terms in {left} and {right}");
        }

        Mapping = mapping;
    }

    public ISimpleTerm Left { get; }

    public ISimpleTerm Right { get; }

    public IImmutableDictionary<Variable, ProhibitedValuesBinding> Mapping { get; }
}
