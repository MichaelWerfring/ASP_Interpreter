using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using System.Collections.Immutable;

namespace asp_interpreter_lib.InternalProgramClasses.Database; 

public class DualClauseDatabase : IDatabase
{
    private FunctorTableRecord _functors;

    private IImmutableDictionary<(string, int), List<IEnumerable<Structure>>> _standardClauses;

    private IImmutableDictionary<(string, int), List<IEnumerable<Structure>>> _dualClauses;

    public DualClauseDatabase(IEnumerable<IEnumerable<Structure>> clauses, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(clauses);
        ArgumentNullException.ThrowIfNull(functors);

        if (clauses.Any((clause) => clause == null || clause.Count() < 1))
        {
            throw new ArgumentException("Must not contain null clauses, or clauses with not at least one term");
        }

        var standardDict = new Dictionary<(string, int), List<IEnumerable<Structure>>>();

        var dualDict = new Dictionary<(string, int), List<IEnumerable<Structure>>>();

        foreach (var clause in clauses)
        {
            var clauseHead = clause.First();

            if
            (
                clauseHead.Functor == functors.NegationAsFailure
                &&
                clauseHead.Children.Count() == 1
                &&
                clauseHead.Children.First() is Structure innerStruct
            )
            {
                AddToDict(clause, (innerStruct.Functor, innerStruct.Children.Count()), dualDict);
            }
            else
            {
                AddToDict(clause, (clauseHead.Functor, clauseHead.Children.Count()), standardDict);
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
        if
        (
            term.Functor == _functors.NegationAsFailure
            &&
            term.Children.Count() == 1
            &&
            term.Children.First() is Structure innerStruct
        )
        {
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

    private IEnumerable<IEnumerable<Structure>> GetMatches
    (
        Structure term,
        IImmutableDictionary<(string, int), List<IEnumerable<Structure>>> dict
    )
    {
        List<IEnumerable<Structure>>? matchingClauses;
        if (!dict.TryGetValue((term.Functor, term.Children.Count()), out matchingClauses))
        {
            return [];
        }

        return matchingClauses;
    }
}
