using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

/// <summary>
/// Updates left by values in right like so:
/// For every variable key in the union of left and right:
/// if only left has value, take left.
/// if only right has value, take right.
/// if left and right has value:
/// if left is prohib and right is prohib, take their union.
/// if left is prohib and right is term, take right.
/// if left is term and right is prohib, fail.
/// if left is term and right is term, fail if they are different or just take right.
/// </summary>
internal class VariableMappingUpdater
{
    public IOption<VariableMapping> Update(VariableMapping left, VariableMapping right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        var variables = left.Keys.Union(right.Keys, TermFuncs.GetSingletonVariableComparer());

        var newPairs = new KeyValuePair<Variable, IVariableBinding>[variables.Count()];

        bool clashEncountered = false;
        Parallel.For(0, newPairs.Length, index =>
        {
            var currentVariable = variables.ElementAt(index);

            IOption<IVariableBinding> resolutionMaybe = Resolve(currentVariable, left, right);
            if (!resolutionMaybe.HasValue)
            {
                clashEncountered = true;          
            }
            else
            {
                newPairs[index] = new KeyValuePair<Variable, IVariableBinding>
                    (currentVariable, resolutionMaybe.GetValueOrThrow());
            }
        });

        if (clashEncountered)
        {
            return new None<VariableMapping>();
        }

        var newValues = ImmutableDictionary.CreateRange(TermFuncs.GetSingletonVariableComparer(), newPairs);

        return new Some<VariableMapping>(new VariableMapping(newValues));
    }

    private IOption<IVariableBinding> Resolve(Variable variable, VariableMapping left, VariableMapping right)
    {
        if (!left.TryGetValue(variable, out IVariableBinding? leftVal))
        {
            return new Some<IVariableBinding>(right[variable]);
        }

        if (!right.TryGetValue(variable, out IVariableBinding? rightVal))
        {
            return new Some<IVariableBinding>(left[variable]);
        }

        return ResolveClash(leftVal, rightVal);
    }

    private IOption<IVariableBinding> ResolveClash(IVariableBinding leftVal, IVariableBinding rightVal)
    {
        if (leftVal is ProhibitedValuesBinding leftProhibs)
        {
            if (rightVal is ProhibitedValuesBinding rightProhibs)
            {
                return new Some<IVariableBinding>
                    (new ProhibitedValuesBinding(leftProhibs.ProhibitedValues.Union(rightProhibs.ProhibitedValues)));
            }
            else if (rightVal is TermBinding rightTermbinding)
            {
                return new Some<IVariableBinding>(rightTermbinding);
            }
            else
            {
                return new None<IVariableBinding>();
            }
        }
        else if (leftVal is TermBinding leftTermbinding)
        {
            if (rightVal is ProhibitedValuesBinding)
            {
                return new None<IVariableBinding>();
            }
            else if (rightVal is TermBinding rightTermbinding)
            {
                if (!leftTermbinding.Term.IsEqualTo(rightTermbinding.Term))
                {
                    return new None<IVariableBinding>();
                }

                return new Some<IVariableBinding>(rightTermbinding);
            }
            else
            {
                return new None<IVariableBinding>();

            }
        }
        else
        {
            return new None<IVariableBinding>();
        }
    }
}
