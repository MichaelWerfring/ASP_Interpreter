using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.Unification.Constructive.Target;

public class ConstructiveTargetBuilder
{
    public ConstructiveTarget Build(ISimpleTerm left, ISimpleTerm right, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        var variables = left.ExtractVariables().Union(right.ExtractVariables()).ToHashSet(new VariableComparer());

        var newDict = mapping.GetProhibitedValueBindings().Where(pair => variables.Contains(pair.Key)).ToDictionary(new VariableComparer());

        foreach (var variable in variables)
        {
            if (!newDict.ContainsKey(variable))
            {
                newDict.Add(variable, new ProhibitedValuesBinding());
            }
        }

        return new ConstructiveTarget(left, right, newDict.ToImmutableDictionary(new VariableComparer()));
    }
}
