using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Util.ErrorHandling.Either;

namespace asp_interpreter_lib.Unification.Constructive.Disunification;

public interface IConstructiveDisunificationAlgorithm
{
    public IEither<DisunificationException, IEnumerable<VariableMapping>> Disunify(ConstructiveTarget target);
}
