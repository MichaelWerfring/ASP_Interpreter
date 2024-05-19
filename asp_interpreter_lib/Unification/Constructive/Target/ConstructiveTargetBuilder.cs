using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target.Builder;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using System.Collections.Immutable;

namespace asp_interpreter_lib.Unification.Constructive.Target;

internal static class ConstructiveTargetBuilder
{
    private static readonly VariableComparer _comparer = new();

    public static IEither<TargetBuildingException, ConstructiveTarget> Build(ISimpleTerm left, ISimpleTerm right, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        var variables = left.ExtractVariables()
            .Union(right.ExtractVariables(), _comparer);

        VariableMapping newMapping = [];

        foreach (Variable variable in variables)
        {
            mapping.TryGetValue(variable, out IVariableBinding? value);

            if (value == null)
            {
                newMapping = newMapping.Add(variable, new ProhibitedValuesBinding());
                continue;
            }

            if (value is TermBinding tb)
            {
                return new Left<TargetBuildingException, ConstructiveTarget>
                    (new TargetBuildingException($"Variable {variable} was bound to term {tb.Term}"));
            }

            newMapping = newMapping.SetItem(variable, value);
        }

        return new Right<TargetBuildingException, ConstructiveTarget>
            (new ConstructiveTarget(left, right, newMapping));
    }
}
