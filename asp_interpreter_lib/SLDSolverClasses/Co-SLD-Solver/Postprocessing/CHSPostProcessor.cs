using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using Medallion.Collections;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

internal partial class CHSPostProcessor
{
    private readonly CHSPostprocessingComparer _comparer;

    public CHSPostProcessor(FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        _comparer = new CHSPostprocessingComparer(functors);
    }

    public CoinductiveHypothesisSet Postprocess(CoinductiveHypothesisSet set)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));

        var results = set.Where
        (
            entry => entry.Term.Enumerate().OfType<Structure>()
                    .All(structure => !structure.Functor.StartsWith('_'))
        )
        .ToList();


        results.Sort(_comparer);

        return new CoinductiveHypothesisSet(results.ToImmutableLinkedList());
    }

}
