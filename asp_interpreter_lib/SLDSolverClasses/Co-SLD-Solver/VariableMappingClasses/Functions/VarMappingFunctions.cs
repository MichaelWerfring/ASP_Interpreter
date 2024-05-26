// <copyright file="VarMappingFunctions.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling;

internal static class VarMappingFunctions
{
    private static readonly VariableMappingMerger Merger = new();
    private static readonly BinaryVariableBindingCaseDeterminer CaseDeterminer = new();
    private static readonly ProhibitedValuesChecker ProhibFilterer = new();
    private static readonly TermBindingChecker TermbindingFilterer = new();

    public static VariableMapping Merge(this IDictionary<Variable, ProhibitedValuesBinding> prohibs, IDictionary<Variable, TermBinding> termbindings)
    {
        return Merger.Merge(prohibs, termbindings);
    }

    public static IBinaryVariableBindingCase DetermineCase(IVariableBinding left, IVariableBinding right)
    {
        return CaseDeterminer.DetermineCase(left, right);
    }

    public static IOption<ProhibitedValuesBinding> ReturnProhibitedValueBindingOrNone(this IVariableBinding variableBinding)
    {
        return ProhibFilterer.ReturnProhibitedValueBindingOrNone(variableBinding);
    }

    public static IOption<TermBinding> ReturnTermbindingOrNone(this IVariableBinding variableBinding)
    {
        return TermbindingFilterer.ReturnTermbindingOrNone(variableBinding);
    }
}