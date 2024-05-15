using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Unification.Constructive.Target;

internal class ConstructiveTargetBuilder
{
    public static IOption<ConstructiveTarget> Build(ISimpleTerm left, ISimpleTerm right, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        var variables = left.ExtractVariables().Union(right.ExtractVariables(), new VariableComparer());

        VariableMapping newMapping = mapping;

        foreach (var variable in variables)
        {
            if (!mapping.TryGetValue(variable, out IVariableBinding? value))
            {
                newMapping = newMapping.Add(variable, new ProhibitedValuesBinding());
            }

            if (value is TermBinding)
            {
                return new None<ConstructiveTarget>();
            }
        }

        return new Some<ConstructiveTarget>(new ConstructiveTarget(left, right, newMapping));
    }
}
