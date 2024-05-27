// <copyright file="VariableMappingSubstituter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

/// <summary>
/// A class for substituting a term using a <see cref="VariableMapping"/>.
/// More convenient to use than extracting the terms and substituting using the appropriate term method.
/// </summary>
public class VariableMappingSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, VariableMapping>
{
    /// <summary>
    /// Substitutes all variables in the term by their value in mapping, in case that they have a termbinding.
    /// </summary>
    /// <param name="term">The term to substitute.</param>
    /// <param name="mapping">The mapping to use for substitution.</param>
    /// <returns>The substituted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// <paramref name="term"/> is null,
    /// <paramref name="mapping"/> is null.</exception>
    public ISimpleTerm SubstituteTerm(ISimpleTerm term, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);

        return term.Accept(this, mapping);
    }

    /// <summary>
    /// Substitutes all variables in the structure by their value in mapping, and returns it as a structure.
    /// </summary>
    /// <param name="term">The term to substitute.</param>
    /// <param name="map">The mapping to use for substitution.</param>
    /// <returns>The substituted structure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// <paramref name="term"/> is null,
    /// <paramref name="map"/> is null.</exception>
    public Structure SubstituteStructure(Structure term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        var newChildren = new ISimpleTerm[term.Children.Count];

        Parallel.For(0, newChildren.Length, index =>
        {
            newChildren[index] = term.Children.ElementAt(index).Accept(this, map);
        });

        return new Structure(term.Functor, newChildren);
    }

    /// <summary>
    /// Visits a variable and substitutes it by its value in the mapping, or just returns the variable in case of no value.
    /// </summary>
    /// <param name="term">The variable to substitute.</param>
    /// <param name="map">The mapping to use for substitution.</param>
    /// <returns>The substituted variable.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// <paramref name="term"/> is null,
    /// <paramref name="map"/> is null.</exception>
    public ISimpleTerm Visit(Variable term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        if (!map.TryGetValue(term, out IVariableBinding? value))
        {
            return term;
        }

        var tbMaybe = VarMappingFunctions.ReturnTermbindingOrNone(value);

        if (!tbMaybe.HasValue)
        {
            return term;
        }

        return tbMaybe.GetValueOrThrow().Term;
    }

    /// <summary>
    /// Visits a structure and substitutes all its variables.
    /// </summary>
    /// <param name="term">The structure to substitute.</param>
    /// <param name="map">The mapping to use for substitution.</param>
    /// <returns>The substituted structure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// <paramref name="term"/> is null,
    /// <paramref name="map"/> is null.</exception>
    public ISimpleTerm Visit(Structure term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return this.SubstituteStructure(term, map);
    }

    /// <summary>
    /// Visits an integer and just returns it.
    /// </summary>
    /// <param name="term">The integer to substitute.</param>
    /// <param name="map">The mapping to use for substitution.</param>
    /// <returns>The input integer.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// <paramref name="term"/> is null,
    /// <paramref name="map"/> is null.</exception>
    public ISimpleTerm Visit(Integer term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return term;
    }
}