// <copyright file="VariableSubstituter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// A class for substituting all the variables in a term.
/// </summary>
public class VariableSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, IDictionary<Variable, ISimpleTerm>>
{
    /// <summary>
    /// Substitutes all the variables in the input term.
    /// </summary>
    /// <param name="term">The input term.</param>
    /// <param name="map">A dictionary containing variables to replacements mapping.</param>
    /// <returns>The substituted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. map is null.</exception>
    public ISimpleTerm Substitute(ISimpleTerm term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return term.Accept(this, map);
    }

    /// <summary>
    /// Substitutes all the variables in the input structure and return it as a structure.
    /// </summary>
    /// <param name="term">The input structure.</param>
    /// <param name="map">A dictionary containing variables to replacements mapping.</param>
    /// <returns>The substituted structure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. map is null.</exception>
    public Structure SubsituteStructure(Structure term, IDictionary<Variable, ISimpleTerm> map)
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
    /// Visits the term and substitutes all its variables.
    /// </summary>
    /// <param name="term">The input term.</param>
    /// <param name="map">A dictionary containing variables to replacements mapping.</param>
    /// <returns>The substituted structure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. map is null.</exception>
    public ISimpleTerm Visit(Structure term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return this.SubsituteStructure(term, map);
    }

    /// <summary>
    /// Visits the term and substitutes all its variables.
    /// </summary>
    /// <param name="term">The input term.</param>
    /// <param name="map">A dictionary containing variables to replacements mapping.</param>
    /// <returns>The substituted structure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. map is null.</exception>
    public ISimpleTerm Visit(Variable term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        map.TryGetValue(term, out ISimpleTerm? value);

        if (value == null)
        {
            return term;
        }

        return value;
    }

    /// <summary>
    /// Visits the term and substitutes all its variables.
    /// </summary>
    /// <param name="term">The input term.</param>
    /// <param name="map">A dictionary containing variables to replacements mapping.</param>
    /// <returns>The substituted structure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. map is null.</exception>
    public ISimpleTerm Visit(Integer term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return term;
    }
}