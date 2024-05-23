using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;

internal class ProhibitedValuesChecker : IVariableBindingVisitor<IOption<ProhibitedValuesBinding>>
{
    public IOption<ProhibitedValuesBinding> ReturnProhibitedValueBindingOrNone(IVariableBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return binding.Accept(this);
    }

    public IOption<ProhibitedValuesBinding> Visit(ProhibitedValuesBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return new Some<ProhibitedValuesBinding>(binding);
    }

    public IOption<ProhibitedValuesBinding> Visit(TermBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return new None<ProhibitedValuesBinding>();
    }
}
