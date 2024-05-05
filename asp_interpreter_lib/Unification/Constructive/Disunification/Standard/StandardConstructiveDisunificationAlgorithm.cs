using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System.Collections.Immutable;

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

    public IEither<DisunificationException, IEnumerable<VariableMapping>> Disunify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        // create new disunifier instance
        var constructiveDisunifier = new ConstructiveDisunifier
            (_doGroundednessCheck, _doDisunifyUnboundVariables, target);

        // if error during disunification, return error.
        var disunifiers = constructiveDisunifier.Disunify();
        if (!disunifiers.IsRight)
        {
            return new Left<DisunificationException, IEnumerable<VariableMapping>>
                (disunifiers.GetLeftOrThrow());
        }

        // build a variableMapping from target's dictionary.
        var immutableBuilder = 
            ImmutableDictionary.CreateBuilder<Variable, IVariableBinding>(new VariableComparer());
        foreach(var pair in target.Mapping)
        {
            immutableBuilder.Add(pair.Key, pair.Value);
        }
        var mapping = new VariableMapping(immutableBuilder.ToImmutable());

        // if disunifiers is empty(target disunifies anyways), just return the mapping
        if (disunifiers.GetRightOrThrow().Count() == 0)
        {
            return new Right<DisunificationException, IEnumerable<VariableMapping>>([mapping]);
        }

        var mappings = new List<VariableMapping>();
        foreach (var disunifier in disunifiers.GetRightOrThrow())
        {
            IImmutableDictionary<Variable, IVariableBinding>? newMapping;
            if (disunifier.IsPositive)
            {
                newMapping = mapping.Mapping.SetItem(disunifier.Variable, new TermBinding(disunifier.Term));
            }
            else
            {
                var prohibitedValueList = (ProhibitedValuesBinding)mapping.Mapping[disunifier.Variable];

                newMapping = mapping.Mapping.SetItem
                (
                    disunifier.Variable,
                    new ProhibitedValuesBinding(prohibitedValueList.ProhibitedValues.Add(disunifier.Term))
                );
            }

            mappings.Add(new VariableMapping(newMapping));
        }

        return new Right<DisunificationException, IEnumerable<VariableMapping>>(mappings);
    }
}
