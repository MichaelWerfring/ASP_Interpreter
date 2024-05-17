using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using System.Collections.Immutable;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Standard;

public class StandardConstructiveDisunificationAlgorithm : IConstructiveDisunificationAlgorithm
{
    private readonly bool _doGroundednessCheck;
    private readonly bool _doDisunifyUnboundVariables;

    public StandardConstructiveDisunificationAlgorithm(bool doGroundednessCheck, bool doDisunifyUnboundVariables)
    {
        _doGroundednessCheck = doGroundednessCheck;
        _doDisunifyUnboundVariables = doDisunifyUnboundVariables;
    }

    public IEither<DisunificationException, IEnumerable<VariableMapping>> Disunify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        // get only the prohibitedValueBindings
        IImmutableDictionary<Variable, ProhibitedValuesBinding> prohitedValueBindings = 
            target.Mapping.GetProhibitedValueBindings();

        // create new disunifier instance
        var constructiveDisunifier = new ConstructiveDisunifier
            (_doGroundednessCheck, _doDisunifyUnboundVariables,target.Left, target.Right, prohitedValueBindings);

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

        // if disunifiers is empty(target disunifies anyways), just return the mapping
        if (!disunifiers.Any())
        {
            return new Right<DisunificationException, IEnumerable<VariableMapping>>([target.Mapping]);
        }

        // create a new mapping for every disunifier where the value is updated by the disunifier value.
        List<VariableMapping> mappings = [];
        foreach (var disunifier in disunifiers)
        {
            VariableMapping newMapping;

            if (disunifier.IsPositive)
            {
                newMapping = target.Mapping.SetItem(disunifier.Variable, new TermBinding(disunifier.Term));
            }
            else
            {
                var prohibitedValueList = prohitedValueBindings[disunifier.Variable];

                newMapping = target.Mapping.SetItem
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
