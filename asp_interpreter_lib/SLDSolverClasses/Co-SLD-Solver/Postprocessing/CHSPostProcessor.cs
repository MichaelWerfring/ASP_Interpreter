// <copyright file="CHSPostProcessor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Unification.Constructive.Unification;
using Medallion.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

/// <summary>
/// Postprocess a <see cref="CoinductiveHypothesisSet"/> instance so it
/// is ordered in the same way as the original s(ASP)-implementation.
/// </summary>
public partial class CHSPostProcessor
{
    private readonly CHSPostprocessingComparer comparer;
    private readonly List<Structure> predicatesToKeep;
    private readonly IConstructiveUnificationAlgorithm algo;

    /// <summary>
    /// Initializes a new instance of the <see cref="CHSPostProcessor"/> class.
    /// </summary>
    /// <param name="functorMapping">The functor mapping for determining how a NaF looks like.</param>
    /// <param name="predicatesToKeep">The predicates to keep.</param>
    /// <param name="algo">The algorithm for unification, used when filtering for predicates to keep.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="functorMapping"/> is null.</exception>
    public CHSPostProcessor(FunctorTableRecord functorMapping, List<Structure> predicatesToKeep, IConstructiveUnificationAlgorithm algo)
    {
        ArgumentNullException.ThrowIfNull(functorMapping, nameof(functorMapping));
        ArgumentNullException.ThrowIfNull(predicatesToKeep, nameof(predicatesToKeep));
        ArgumentNullException.ThrowIfNull(algo, nameof(algo));

        this.comparer = new CHSPostprocessingComparer(functorMapping);
        this.predicatesToKeep = predicatesToKeep;
        this.algo = algo;
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


        resultList = this.FilterForPredicatesToKeep(resultList);

        return new CoinductiveHypothesisSet(resultList.ToImmutableLinkedList());
    }

    private List<CHSEntry> FilterForPredicatesToKeep(List<CHSEntry> resultList)
    {
        if (this.predicatesToKeep.Count == 0)
        {
            return resultList;
        }

        var emptyMappingForFiltering = new VariableMapping();

        var filteredEntries = resultList.Where(entry =>
        {
            return this.predicatesToKeep.Any(pred =>
            {
                var target = ConstructiveTargetBuilder.Build(pred, entry.Term, emptyMappingForFiltering).GetRightOrThrow();

                return this.algo.Unify(target).HasValue;
            });
        });

        return filteredEntries.ToList();
    }
}