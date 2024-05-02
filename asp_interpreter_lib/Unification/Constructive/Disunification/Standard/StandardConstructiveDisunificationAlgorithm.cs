using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;
using asp_interpreter_lib.Util.ErrorHandling.Either;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Standard;

public class StandardConstructiveDisunificationAlgorithm : IConstructiveDisunificationAlgorithm
{
    private bool _doGroundednessCheck;
    private bool _doDisunifyUnboundVariables;

    public StandardConstructiveDisunificationAlgorithm(bool doGroundednessCheck, bool doDisunifyUnboundVariables)
    {
        _doGroundednessCheck = doGroundednessCheck;
        _doDisunifyUnboundVariables = doDisunifyUnboundVariables;
    }

    public IEither<DisunificationException, IEnumerable<VariableDisunifier>> Disunify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        var disunifier = new ConstructiveDisunifier(_doGroundednessCheck, _doDisunifyUnboundVariables, target);

        return disunifier.Disunify();
    }
}
