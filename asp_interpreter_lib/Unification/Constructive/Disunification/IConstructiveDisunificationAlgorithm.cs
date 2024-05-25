using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling.Either;

namespace Asp_interpreter_lib.Unification.Constructive.Disunification;

public interface IConstructiveDisunificationAlgorithm
{
    public IEither<DisunificationException, IEnumerable<VariableMapping>> Disunify(ConstructiveTarget target);
}
