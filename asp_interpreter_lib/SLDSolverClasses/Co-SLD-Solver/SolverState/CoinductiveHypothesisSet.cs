using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoinductiveHypothesisSet
{
    public CoinductiveHypothesisSet()
    {
        Terms = ImmutableHashSet.Create<ISimpleTerm>(new SimpleTermEqualityComparer());
    }

    public CoinductiveHypothesisSet(ImmutableHashSet<ISimpleTerm> termSet)
    {
        ArgumentNullException.ThrowIfNull(termSet);
        if (termSet.KeyComparer is not SimpleTermEqualityComparer)
        {
            throw new ArgumentException("Must contain correct comparer.");
        }

        Terms = termSet;
    }

    public ImmutableHashSet<ISimpleTerm> Terms { get; }

    public override string ToString()
    {
        return $"{{ {Terms.ToList().ToList()} }}";
    }
}
