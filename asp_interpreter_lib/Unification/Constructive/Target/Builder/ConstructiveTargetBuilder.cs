using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling.Either;
using System.Collections.Immutable;

namespace Asp_interpreter_lib.Unification.Constructive.Target.Builder;

public static class ConstructiveTargetBuilder
{
    private static readonly ValueRetriever _retriever = new();

    public static IEither<TargetBuildingException, ConstructiveTarget> Build(ISimpleTerm left, ISimpleTerm right, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        var variables = left.ExtractVariables().Union(right.ExtractVariables(), TermFuncs.GetSingletonVariableComparer());

        var newMapping = ImmutableDictionary.Create<Variable, ProhibitedValuesBinding>(TermFuncs.GetSingletonVariableComparer());

        foreach (Variable variable in variables)
        {
            mapping.TryGetValue(variable, out IVariableBinding? value);

            if (value == null)
            {
                newMapping = newMapping.Add(variable, new ProhibitedValuesBinding());
                continue;
            }

            var valueEither = _retriever.GetProhibitedValuesOrError(variable, mapping);

            if (!valueEither.IsRight)
            {
                return new Left<TargetBuildingException, ConstructiveTarget>(valueEither.GetLeftOrThrow());
            }

            newMapping = newMapping.SetItem(variable, valueEither.GetRightOrThrow());
        }

        return new Right<TargetBuildingException, ConstructiveTarget>
            (new ConstructiveTarget(left, right, newMapping));
    }
}
