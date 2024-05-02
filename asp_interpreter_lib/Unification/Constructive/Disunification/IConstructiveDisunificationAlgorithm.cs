using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.ErrorHandling.Either;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;

namespace asp_interpreter_lib.Unification.Constructive.Disunification;

public interface IConstructiveDisunificationAlgorithm
{
    public IEither<DisunificationException, IEnumerable<VariableDisunifier>> Disunify(ConstructiveTarget target);
}
