// <copyright file="DualClauseDatabase.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.Database;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using System.Collections.Immutable;

/// <summary>
/// A class that acts as a database for fast lookup of clauses, with an additional seperation between normal clauses and dual clauses.
/// </summary>
public class DualClauseDatabase : IDatabase
{
    private readonly FunctorTableRecord functors;
    private readonly IImmutableDictionary<(string, int), List<IEnumerable<Structure>>> standardClauses;
    private readonly IImmutableDictionary<(string, int), List<IEnumerable<Structure>>> dualClauses;

    /// <summary>
    /// Initializes a new instance of the <see cref="DualClauseDatabase"/> class.
    /// </summary>
    /// <param name="clauses"> The clauses for the database.</param>
    /// <param name="functors">A record of functor, so the database knows what negation as failure looks like.</param>
    /// <exception cref="ArgumentNullException">Thrown when..
    /// .. <paramref name="clauses"/> is null.
    /// .. <paramref name="functors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="clauses"/> contains nulls, or empty clauses.</exception>
    public DualClauseDatabase(IEnumerable<IEnumerable<Structure>> clauses, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(clauses);
        ArgumentNullException.ThrowIfNull(functors);

        if (clauses.Any(clause => clause == null || !clause.Any()))
        {
            throw new ArgumentException("Must not contain null clauses, or clauses with not at least one term");
        }

        var standardDict = new Dictionary<(string, int), List<IEnumerable<Structure>>>();

        var dualDict = new Dictionary<(string, int), List<IEnumerable<Structure>>>();

        foreach (var clause in clauses)
        {
            var clauseHead = clause.First();

            if (clauseHead.IsNegated(functors))
            {
                Structure innerTerm = clauseHead.NegateTerm(functors);

                this.AddToDict(clause, (innerTerm.Functor, innerTerm.Children.Count), dualDict);
            }
            else
            {
                this.AddToDict(clause, (clauseHead.Functor, clauseHead.Children.Count), standardDict);
            }
        }

        this.functors = functors;

        this.standardClauses = standardDict.ToImmutableDictionary();

        this.dualClauses = dualDict.ToImmutableDictionary();
    }

    /// <summary>
    /// Enumerates potentially unifying clauses for the input term.
    /// </summary>
    /// <param name="term">A term to seek unfying clauses for.</param>
    /// <returns>An enumeration of clauses that might unify with the input term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IEnumerable<IEnumerable<Structure>> GetPotentialUnifications(Structure term)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        IEnumerable<IEnumerable<Structure>> matchingClauses;
        if (term.IsNegated(this.functors))
        {
            Structure innerStruct = term.NegateTerm(this.functors);

            matchingClauses = this.GetMatches(innerStruct, this.dualClauses);
        }
        else
        {
            matchingClauses = this.GetMatches(term, this.standardClauses);
        }

        foreach (var clause in matchingClauses)
        {
            yield return clause;
        }
    }

    private void AddToDict(
        IEnumerable<Structure> clause,
        (string Functor, int Arity) functorArityPair,
        Dictionary<(string Functor, int Arity), List<IEnumerable<Structure>>> dict)
    {
        if (dict.TryGetValue(functorArityPair, out List<IEnumerable<Structure>>? list))
        {
            list.Add(clause);
        }
        else
        {
            dict.Add(functorArityPair,[clause]);
        }
    }

    private List<IEnumerable<Structure>> GetMatches(
        Structure term,
        IImmutableDictionary<(string Functor, int Arity), List<IEnumerable<Structure>>> dict)
    {
        List<IEnumerable<Structure>>? matchingClauses;
        if (!dict.TryGetValue((term.Functor, term.Children.Count), out matchingClauses))
        {
            return[];
        }

        return matchingClauses;
    }
}