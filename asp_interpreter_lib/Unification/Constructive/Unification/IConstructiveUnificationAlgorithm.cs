using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Unification.Constructive.Unification;

public interface IConstructiveUnificationAlgorithm
{
    public IOption<VariableMapping> Unify(ConstructiveTarget target);
}
