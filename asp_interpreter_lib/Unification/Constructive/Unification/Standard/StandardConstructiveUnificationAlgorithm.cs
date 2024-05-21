using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

public class StandardConstructiveUnificationAlgorithm : IConstructiveUnificationAlgorithm
{
    private readonly bool _doOccursCheck;

    private readonly ConstructiveVariableSubstitutor _substituter;
    private readonly SubstitutionApplier _subApplier;
    private readonly ProhibitedValuesUpdater _prohibsUpdater;
    private readonly CaseDeterminer _caseDeterminer;

    public StandardConstructiveUnificationAlgorithm(bool doOccursCheck)
    {
        _doOccursCheck = doOccursCheck;
        _substituter = new();
        _subApplier = new();
        _prohibsUpdater = new();
        _caseDeterminer = new();
    }

    /// <summary>
    /// Try to unify two terms, based on mapping of variables to prohibited value lists.
    /// Exceptions are thrown if mapping does not contain a value for each variable in left and right.
    /// </summary>
    public IOption<VariableMapping> Unify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        var constructiveUnifier = new ConstructiveUnifier
        (
            _doOccursCheck,
            target,
            _substituter,
            _subApplier,
            _prohibsUpdater,
            _caseDeterminer
        );

        return constructiveUnifier.Unify();
    }
}
