﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class SolverStateUpdater
{
    public CallStack UpdateCallstack(CallStack callStack, VariableMapping map)
    {
        var newCalls = new ISimpleTerm[callStack.Count()];
        Parallel.For(0, newCalls.Length, index =>
        {
            newCalls[index] = map.ApplySubstitution
            (callStack.ElementAt(newCalls.Length - 1 - index));
        });
        
        var newCallstack = new CallStack(ImmutableStack.CreateRange(newCalls));

        return newCallstack;
    }

    public CoinductiveHypothesisSet UpdateCHS(CoinductiveHypothesisSet set, VariableMapping map)
    {
        var newCH = new CHSEntry[set.Count];
        Parallel.For(0, newCH.Length, index =>
        {
            var currentEntry = set.ElementAt(index);

            newCH[index] = new CHSEntry
                (map.ApplySubstitution(currentEntry.Term), currentEntry.HasSucceded);
        });

        return new CoinductiveHypothesisSet([.. newCH]);
    }
}