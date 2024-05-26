// <copyright file="CHSPostProcessor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Medallion.Collections;

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
