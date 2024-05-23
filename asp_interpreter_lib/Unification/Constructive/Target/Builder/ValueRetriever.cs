using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling.Either;

namespace asp_interpreter_lib.Unification.Constructive.Target.Builder;

internal class ValueRetriever : IVariableBindingVisitor<IEither<TargetBuildingException, ProhibitedValuesBinding>>
{
    public IEither<TargetBuildingException, ProhibitedValuesBinding> GetProhibitedValuesOrError(Variable variable, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        if (!mapping.TryGetValue(variable, out IVariableBinding? value))
        {
            return new Right<TargetBuildingException, ProhibitedValuesBinding>(new ProhibitedValuesBinding());
        }

        return value.Accept(this);
    }

    public IEither<TargetBuildingException, ProhibitedValuesBinding> Visit(ProhibitedValuesBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding);

        return new Right<TargetBuildingException, ProhibitedValuesBinding>(binding);
    }

    public IEither<TargetBuildingException, ProhibitedValuesBinding> Visit(TermBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding);

        return new Left<TargetBuildingException, ProhibitedValuesBinding>
             (new TargetBuildingException($"Failed to build target: mapping contained termbinding."));
    }
}
