using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Concatenation.Exceptions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class VariableMappingUpdater : IVariableBindingArgumentVisitor<IVariableBinding, (Variable,IVariableBinding)>
{
    /// <summary>
    /// Updates variableMapping with another VariableMapping.
    /// For a single variable:
    /// if only one variableMapping contains the variable, then take that one.
    /// if one maps to a term and the other to a prohibited values list, take term.
    /// if both map to prohibited values list, take the second one BUT if right does not contain all terms of left, then fail.
    /// if the 2 maps map to different termBinding, then fail. Otherwise, take one.
    /// </summary>
    public IEither<UpdateException, VariableMapping> Update(VariableMapping first, VariableMapping second)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));

        var variables = first.Mapping.Keys.Union(second.Mapping.Keys, new VariableComparer());

        Dictionary<Variable, IVariableBinding> newDict;
        try
        {
            newDict = variables.Select(x => ConcatenatePairs(x, first, second))
                            .ToDictionary(new VariableComparer());
        }
        catch (UpdateException e)
        {
            return new Left<UpdateException, VariableMapping>(e);
        }

        return new Right<UpdateException, VariableMapping>(new VariableMapping(newDict.ToImmutableDictionary(new VariableComparer())));
    }

    private (Variable, IVariableBinding) ConcatenatePairs(Variable x, VariableMapping first, VariableMapping second)
    {
        IVariableBinding? leftBinding;
        if (!first.Mapping.TryGetValue(x, out leftBinding))
        {
            return (x, second.Mapping[x]);
        }

        IVariableBinding? rightBinding;
        if (!second.Mapping.TryGetValue(x, out rightBinding))
        {
            return (x, first.Mapping[x]);
        }

        return (x, leftBinding.Accept(this, (x, rightBinding)));
    }

    // visitor
    public IVariableBinding Visit(ProhibitedValuesBinding first, (Variable, IVariableBinding) second)
    {     
        if (second.Item2 is TermBinding termBinding)
        {
            return termBinding;
        }

        var rightAsProhibitedValues = (ProhibitedValuesBinding)second.Item2;

        if (first.ProhibitedValues.Any(term => !rightAsProhibitedValues.ProhibitedValues.Contains(term)))
        {
            throw new ProhibitedValuesMismatchException
                ($"Mismatch for variable {second.Item1}: " +
                $"{nameof(second)}'s prohibited values list did not contain " +
                $"all terms of {nameof(first)}");
        }

        return rightAsProhibitedValues;
    }

    public IVariableBinding Visit(TermBinding first, (Variable, IVariableBinding) second)
    {
        if (second.Item2 is ProhibitedValuesBinding)
        {
            return first;
        }

        var rightAsTermBinding = (TermBinding)second.Item2;

        if (!first.Term.IsEqualTo(rightAsTermBinding.Term))
        {
            throw new TermClashException
            (
                $"Clash between {first.Term.ToString()}" +
                $" and {rightAsTermBinding.Term.ToString()}" +
                $" for variable {second.Item1}"
            );
        }

        return first;
    }
}
