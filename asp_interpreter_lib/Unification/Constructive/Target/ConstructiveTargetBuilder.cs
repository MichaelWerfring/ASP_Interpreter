using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.Unification.Constructive.Target;

public class ConstructiveTargetBuilder
{
    public ConstructiveTarget Build(ISimpleTerm left, ISimpleTerm right, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        var variables = left.ToList()
                            .Union(right.ToList())
                            .Where(x => x is Variable)
                            .Select(x => (Variable)x)
                            .ToImmutableHashSet(new VariableComparer());

        var newDict = new Dictionary<Variable, ProhibitedValuesBinding>(new VariableComparer());
        foreach (var variable in variables)
        {
            IVariableBinding? binding;
            mapping.Mapping.TryGetValue(variable, out binding);

            if (binding == null)
            {
                newDict.Add((Variable)variable.Clone(), new ProhibitedValuesBinding(ImmutableHashSet.Create<ISimpleTerm>(new SimpleTermEqualityComparer())));
                continue;
            }

            if (binding is TermBinding)
            {
                throw new ArgumentException
                    ($"{nameof(mapping)} must not contain term bindings for any variables in {nameof(left)} or {nameof(right)}");
            }

            newDict.Add((Variable)variable.Clone(), (ProhibitedValuesBinding)binding);
        }

        return new ConstructiveTarget(left.Clone(), right.Clone(), newDict.ToImmutableDictionary(new VariableComparer()));
    }
}
