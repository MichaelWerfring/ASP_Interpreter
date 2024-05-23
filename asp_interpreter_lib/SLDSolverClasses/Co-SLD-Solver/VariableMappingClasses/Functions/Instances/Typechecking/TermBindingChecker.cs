using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;

internal class TermBindingChecker : IVariableBindingVisitor<IOption<TermBinding>>
{
    public IOption<TermBinding> ReturnTermbindingOrNone(IVariableBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return binding.Accept(this);
    }

    public IOption<TermBinding> Visit(ProhibitedValuesBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return new None<TermBinding>();
    }

    public IOption<TermBinding> Visit(TermBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return new Some<TermBinding>(binding);
    }
}
