// <copyright file="SolverStateUpdater.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

/// <summary>
/// A class for updating callstack and chs.
/// </summary>
public class SolverStateUpdater
{
    /// <summary>
    /// Updates a callstack based on a new mapping.
    /// </summary>
    /// <param name="callStack">The input callstack</param>
    /// <param name="map">The new mapping to update with.</param>
    /// <returns>An updated callstack.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="callStack"/> is null,
    /// ..<paramref name="map"/> is null.</exception>
    public CallStack UpdateCallstack(CallStack callStack, VariableMapping map)
    {
        var newCalls = new Structure[callStack.Count()];
        Parallel.For(0, newCalls.Length, index =>
        {
            newCalls[index] = map.ApplySubstitution(
                callStack.ElementAt(newCalls.Length - 1 - index));
        });

        var newCallstack = new CallStack(ImmutableStack.CreateRange(newCalls));

        return newCallstack;
    }

    /// <summary>
    /// Updates a coinductive hypothesis set based on a new mapping.
    /// </summary>
    /// <param name="set">The input callstack</param>
    /// <param name="map">The new mapping to update with.</param>
    /// <returns>An updated chs.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="set"/> is null,
    /// ..<paramref name="map"/> is null.</exception>
    public CoinductiveHypothesisSet UpdateCHS(CoinductiveHypothesisSet set, VariableMapping map)
    {
        var newCH = new CHSEntry[set.Count];
        Parallel.For(0, newCH.Length, index =>
        {
            var currentEntry = set.ElementAt(index);

            newCH[index] = new CHSEntry(map.ApplySubstitution(currentEntry.Term), currentEntry.HasSucceded);
        });

        return new CoinductiveHypothesisSet([.. newCH]);
    }
}