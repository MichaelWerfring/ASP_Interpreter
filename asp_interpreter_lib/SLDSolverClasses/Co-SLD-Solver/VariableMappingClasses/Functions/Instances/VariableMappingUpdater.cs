using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;
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
internal class VariableMappingUpdater : IBinaryVariableBindingCaseVisitor<IOption<IVariableBinding>>
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

    public IOption<IVariableBinding> Visit(ProhibValsProhibValsCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase, nameof(binaryCase));

        return new Some<IVariableBinding>
            (new ProhibitedValuesBinding(binaryCase.Left.ProhibitedValues.Union(binaryCase.Right.ProhibitedValues)));
    }

    public IOption<IVariableBinding> Visit(ProhibValsTermBindingCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase, nameof(binaryCase));

        return new Some<IVariableBinding>(binaryCase.Right);
    }

    public IOption<IVariableBinding> Visit(TermBindingProhibValsCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase, nameof(binaryCase));

        return new None<IVariableBinding>();
    }

    public IOption<IVariableBinding> Visit(TermBindingTermBindingCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase, nameof(binaryCase));

        if (!binaryCase.Left.Term.IsEqualTo(binaryCase.Right.Term))
        {
            return new None<IVariableBinding>();
        }

        return new Some<IVariableBinding>(binaryCase.Right);
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

        var typeCase = VarMappingFunctions.DetermineCase(leftVal, rightVal);

        return typeCase.Accept(this);
    }
}
