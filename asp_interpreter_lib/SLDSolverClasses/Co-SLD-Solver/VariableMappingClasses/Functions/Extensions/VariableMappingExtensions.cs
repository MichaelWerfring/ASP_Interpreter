using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;

public static class VariableMappingExtensions
{
    private static readonly VariableMappingSplitter _splitter = new();
    private static readonly VariableMappingSubstituter _substituter = new();
    private static readonly VariableMappingFlattener _flattener = new();
    private static readonly VariableMappingUpdater _updater = new();
    private static readonly TransitiveVariableMappingResolver _toProhibResolver = new(true);
    private static readonly TransitiveVariableMappingResolver _toLastVariableResolver = new(false);

    public static IImmutableDictionary<Variable, TermBinding> GetTermBindings(this VariableMapping mapping)
    {
        return _splitter.GetTermBindings(mapping);
    }

    public static IImmutableDictionary<Variable, ProhibitedValuesBinding> GetProhibitedValueBindings(this VariableMapping mapping)
    {
        return _splitter.GetProhibitedValueBindings(mapping);
    }

    public static VariableMapping Flatten(this VariableMapping mapping)
    {
        return _flattener.Flatten(mapping);
    }

    public static IOption<VariableMapping> Update(this VariableMapping left, VariableMapping right)
    {
        return _updater.Update(left, right);
    }

    public static IOption<IVariableBinding> Resolve(this VariableMapping mapping, Variable var, bool doProhibitedValueResolution)
    {
        if (doProhibitedValueResolution)
        {
            return _toProhibResolver.Resolve(var, mapping);
        }
        else
        {
            return _toLastVariableResolver.Resolve(var, mapping);
        }
    }

    public static ISimpleTerm ApplySubstitution(this VariableMapping mapping, ISimpleTerm term)
    {
        return _substituter.Substitute(term, mapping);
    }

    public static Structure ApplySubstitution(this VariableMapping mapping, Structure term)
    {
        return (_substituter.Visit(term, mapping) as Structure)!;
    }
}
