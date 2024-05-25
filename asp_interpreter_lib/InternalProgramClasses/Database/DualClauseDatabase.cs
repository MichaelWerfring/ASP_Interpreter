using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using System.Collections.Immutable;

namespace Asp_interpreter_lib.InternalProgramClasses.Database;

public class DualClauseDatabase : IDatabase
{
    private readonly FunctorTableRecord _functors;
    private readonly IImmutableDictionary<(string, int), List<IEnumerable<Structure>>> _standardClauses;
    private readonly IImmutableDictionary<(string, int), List<IEnumerable<Structure>>> _dualClauses;

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

                AddToDict(clause, (innerTerm.Functor, innerTerm.Children.Count), dualDict);
            }
            else
            {
                AddToDict(clause, (clauseHead.Functor, clauseHead.Children.Count), standardDict);
            }
        }

        _functors = functors;

        _standardClauses = standardDict.ToImmutableDictionary();

        _dualClauses = dualDict.ToImmutableDictionary();
    }

    public IEnumerable<IEnumerable<Structure>> GetPotentialUnifications(Structure term)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        IEnumerable<IEnumerable<Structure>> matchingClauses;
        if (term.IsNegated(_functors))
        {
            Structure innerStruct = term.NegateTerm(_functors);

            matchingClauses = GetMatches(innerStruct, _dualClauses);
        }
        else
        {
            matchingClauses = GetMatches(term, _standardClauses);
        }

        foreach (var clause in matchingClauses)
        {
            yield return clause;
        }
    }

    private void AddToDict
    (
        IEnumerable<Structure> clause,
        (string, int) functorArityPair, 
        Dictionary<(string, int), List<IEnumerable<Structure>>> dict
    )
    {
        if (dict.TryGetValue(functorArityPair, out List<IEnumerable<Structure>>? list))
        {
            list.Add(clause);
        }
        else
        {
            dict.Add(functorArityPair, [clause]);
        }
    }

    private List<IEnumerable<Structure>> GetMatches
    (
        Structure term,
        IImmutableDictionary<(string, int), List<IEnumerable<Structure>>> dict
    )
    {
        List<IEnumerable<Structure>>? matchingClauses;
        if (!dict.TryGetValue((term.Functor, term.Children.Count), out matchingClauses))
        {
            return [];
        }

        return matchingClauses;
    }
}
