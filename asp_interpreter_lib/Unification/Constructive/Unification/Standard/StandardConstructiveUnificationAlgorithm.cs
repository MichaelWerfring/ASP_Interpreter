using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

public class StandardConstructiveUnificationAlgorithm : IConstructiveUnificationAlgorithm
{
    private readonly ConstructiveVariableSubstitutor _substituter;
    private readonly bool _doOccursCheck;
    private readonly VariableComparer _varComparer;

    public StandardConstructiveUnificationAlgorithm
    (
        bool doOccursCheck,
        ConstructiveVariableSubstitutor substitutor,
        VariableComparer varComparer
    )
    {
        ArgumentNullException.ThrowIfNull(substitutor);
        ArgumentNullException.ThrowIfNull(varComparer);

        _doOccursCheck = doOccursCheck;
        _substituter = substitutor;
        _varComparer = varComparer;
    }

    /// <summary>
    /// Try to unify two terms, based on mapping of variables to prohibited value lists.
    /// Exceptions are thrown if mapping does not contain a value for each variable in left and right.
    /// </summary>
    public IOption<VariableMapping> Unify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        var constructiveUnifier = new ConstructiveUnifier(_doOccursCheck, target, _substituter, _varComparer);

        return constructiveUnifier.Unify();
    }
}
