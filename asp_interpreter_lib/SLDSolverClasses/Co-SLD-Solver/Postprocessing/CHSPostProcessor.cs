using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

internal partial class CHSPostProcessor 
{
    public CoinductiveHypothesisSet Postprocess(CoinductiveHypothesisSet set)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));

        var list = new List<CHSEntry>();

        var results = set.Where
        (
            entry => entry.Term.Enumerate().OfType<Structure>()
                    .All(structure => MyRegex().IsMatch(structure.Functor))
        );

        return new CoinductiveHypothesisSet(results.ToImmutableList());
    }

    [GeneratedRegex("^[a-zA-Z]+$")]
    private static partial Regex MyRegex();
}
