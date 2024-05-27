// <copyright file="CHSPostProcessor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Medallion.Collections;
using System.Collections.Immutable;

/// <summary>
/// Postprocess a <see cref="CoinductiveHypothesisSet"/> instance so it
/// is ordered in the same way as the original s(ASP)-implementation.
/// </summary>
internal partial class CHSPostProcessor
{
    private readonly CHSPostprocessingComparer comparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CHSPostProcessor"/> class.
    /// </summary>
    /// <param name="functorMapping">The functor mapping for determining how a NaF looks like.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="functorMapping"/> is null.</exception>
    public CHSPostProcessor(FunctorTableRecord functorMapping)
    {
        ArgumentNullException.ThrowIfNull(functorMapping, nameof(functorMapping));

        this.comparer = new CHSPostprocessingComparer(functorMapping);
    }

    /// <summary>
    /// Postprocesses a <see cref="CoinductiveHypothesisSet"/> for output.
    /// </summary>
    /// <param name="set">The input chs.</param>
    /// <returns>A postprocessed chs.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="set"/> is null.</exception>
    public CoinductiveHypothesisSet Postprocess(CoinductiveHypothesisSet set)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));

        var results = set.Where(
            entry => entry.Term.Enumerate().OfType<Structure>()
                    .All(structure => !structure.Functor.StartsWith('_')))
                    .ToImmutableSortedSet(this.comparer);

        var resultList = results.ToList();
        resultList.Sort(this.comparer);

        return new CoinductiveHypothesisSet(resultList.ToImmutableLinkedList());
    }
}