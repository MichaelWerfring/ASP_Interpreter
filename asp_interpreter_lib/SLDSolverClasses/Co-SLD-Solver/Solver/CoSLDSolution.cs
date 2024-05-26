// <copyright file="CoSLDSolution.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

/// <summary>
/// Represents a solution to a query.
/// </summary>
public class CoSLDSolution
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CoSLDSolution"/> class.
    /// </summary>
    /// <param name="chsEntries">The solution chs entries.</param>
    /// <param name="mapping">The solution mapping.</param>
    public CoSLDSolution(IEnumerable<ISimpleTerm> chsEntries, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(chsEntries, nameof(chsEntries));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        this.CHSEntries = chsEntries;
        this.SolutionMapping = mapping;
    }

    /// <summary>
    /// Gets the chs entries.
    /// </summary>
    public IEnumerable<ISimpleTerm> CHSEntries { get; }

    /// <summary>
    /// Gets the solution mapping.
    /// </summary>
    public VariableMapping SolutionMapping { get; }
}