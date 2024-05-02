using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.Unification.Constructive.Unification;

public interface IConstructiveUnificationAlgorithm
{
    public IOption<VariableMapping> Unify(ConstructiveTarget target);
}
