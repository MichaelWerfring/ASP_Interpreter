using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Standard;

public class StandardConstructiveDisunificationAlgorithm : IConstructiveDisunificationAlgorithm
{
    private readonly CaseDeterminer _caseDeterminer;

    private readonly bool _doGroundednessCheck;
    private readonly bool _doDisunifyUnboundVariables;

    public StandardConstructiveDisunificationAlgorithm(bool doGroundednessCheck, bool doDisunifyUnboundVariables)
    {
        _caseDeterminer = new();

        _doGroundednessCheck = doGroundednessCheck;
        _doDisunifyUnboundVariables = doDisunifyUnboundVariables;
    }

    public IEither<DisunificationException, IEnumerable<VariableMapping>> Disunify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        // create new disunifier instance
        var constructiveDisunifier = new ConstructiveDisunifier
        (
            _doGroundednessCheck,
            _doDisunifyUnboundVariables,
            target.Left,
            target.Right,
            target.Mapping,
            _caseDeterminer
        );

        // if error during disunification, return error.
        var disunifiersEither = constructiveDisunifier.Disunify();
        IEnumerable<DisunificationResult> disunifiers;
        try
        {
            disunifiers = disunifiersEither.GetRightOrThrow();
        }
        catch
        {
            return new Left<DisunificationException, IEnumerable<VariableMapping>>
                           (disunifiersEither.GetLeftOrThrow());
        }

        // wrap input mapping into a more generic variable mapping
        var inputMappingAsVariableMapping = new VariableMapping(target.Mapping);

        // if disunifiers is empty(target disunifies anyways), just return the mapping
        if (!disunifiers.Any())
        {
            return new Right<DisunificationException, IEnumerable<VariableMapping>>([inputMappingAsVariableMapping]);
        }

        // create a new mapping for every disunifier where the value is updated by the disunifier value.
        List<VariableMapping> mappings = [];
        foreach (var disunifier in disunifiers)
        {
            VariableMapping newMapping;

            if (disunifier.IsPositive)
            {
                newMapping = inputMappingAsVariableMapping.SetItem(disunifier.Variable, new TermBinding(disunifier.Term));
            }
            else
            {
                var prohibitedValueList = target.Mapping[disunifier.Variable];

                newMapping = inputMappingAsVariableMapping.SetItem
                (
                    disunifier.Variable,
                    new ProhibitedValuesBinding(prohibitedValueList.ProhibitedValues.Add(disunifier.Term))
                );
            }

            mappings.Add(newMapping);
        }

        return new Right<DisunificationException, IEnumerable<VariableMapping>>(mappings);
    }
}
