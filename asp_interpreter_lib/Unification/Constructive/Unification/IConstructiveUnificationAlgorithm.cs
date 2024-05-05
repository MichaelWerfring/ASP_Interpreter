using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Unification.Constructive.Unification;

public interface IConstructiveUnificationAlgorithm
{
    public IOption<VariableMapping> Unify(ConstructiveTarget target);
}
