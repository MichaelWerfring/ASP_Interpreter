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

/// <summary>
/// A static class that contains commonly used functions involving <see cref="VariableMapping"/>.
/// </summary>
internal static class VarMappingFunctions
{
    private static readonly VariableMappingMerger Merger = new();
    private static readonly BinaryVariableBindingCaseDeterminer CaseDeterminer = new();
    private static readonly ProhibitedValuesChecker ProhibFilterer = new();
    private static readonly TermBindingChecker TermbindingFilterer = new();

    /// <summary>
    /// Merges two dictionaries containing <see cref="Variable"/> to <see cref="ProhibitedValuesBinding"/> mappings
    /// and <see cref="Variable"/> to <see cref="TermBinding"/> mappings into a <see cref="VariableMapping"/>.
    /// In case of clashes, termbindings are preferred.
    /// </summary>
    /// <param name="prohibs">The first dictionary.</param>
    /// <param name="termbindings">The second dictionary.</param>
    /// <returns>A variableMapping containg the merging of both inputs.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="prohibs"/> is null,
    /// ..<paramref name="termbindings"/> is null.</exception>
    public static VariableMapping Merge(this IDictionary<Variable, ProhibitedValuesBinding> prohibs, IDictionary<Variable, TermBinding> termbindings)
    {
        return Merger.Merge(prohibs, termbindings);
    }

    /// <summary>
    /// Determines the case of two input arguments.
    /// </summary>
    /// <param name="left">The left argument.</param>
    /// <param name="right">The right argument.</param>
    /// <returns>A case depending on the types of the two input arguments.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="left"/> is null,
    /// ..<paramref name="right"/> is null.</exception>
    public static IBinaryVariableBindingCase DetermineCase(IVariableBinding left, IVariableBinding right)
    {
        return CaseDeterminer.DetermineCase(left, right);
    }

    /// <summary>
    /// Checks if a <see cref="IVariableBinding"/> is of type <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="variableBinding">The binding to check.</param>
    /// <returns>The binding as a <see cref="ProhibitedValuesBinding"/>, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="variableBinding"/> is null.</exception>
    public static IOption<ProhibitedValuesBinding> ReturnProhibitedValueBindingOrNone(this IVariableBinding variableBinding)
    {
        return ProhibFilterer.ReturnProhibitedValueBindingOrNone(variableBinding);
    }

    /// <summary>
    /// Checks if a <see cref="IVariableBinding"/> is of type <see cref="TermBinding"/>.
    /// </summary>
    /// <param name="variableBinding">The binding to check.</param>
    /// <returns>The binding as a <see cref="TermBinding"/>, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="variableBinding"/> is null.</exception>
    public static IOption<TermBinding> ReturnTermbindingOrNone(this IVariableBinding variableBinding)
    {
        return TermbindingFilterer.ReturnTermbindingOrNone(variableBinding);
    }
}