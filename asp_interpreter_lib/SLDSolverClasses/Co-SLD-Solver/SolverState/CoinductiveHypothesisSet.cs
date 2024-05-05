using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoinductiveHypothesisSet
{
    public CoinductiveHypothesisSet(IImmutableSet<ISimpleTerm> termSet)
    {
        ArgumentNullException.ThrowIfNull(termSet);

        Terms = termSet;
    }

    public IImmutableSet<ISimpleTerm> Terms { get; }
}
