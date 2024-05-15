

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

internal class CHSPostProcessor 
{
    public CoinductiveHypothesisSet Postprocess(CoinductiveHypothesisSet set)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));

        var legalPattern = @"^[a-zA-Z]+$";

        var list = new List<CHSEntry>();

        var results = set.Where
        (
            entry => entry.Term.Enumerate().OfType<Structure>()
                    .All(structure => Regex.IsMatch(structure.Functor, legalPattern))
        );

        return new CoinductiveHypothesisSet(results.ToImmutableList());
    }
}
