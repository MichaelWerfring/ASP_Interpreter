// <copyright file="CoinductiveCheckingResult.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

public class CoinductiveCheckingResult
{
    public CoinductiveCheckingResult(Structure target, VariableMapping map, SuccessType successType) 
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(map, nameof(map));

        Target = target;
        Mapping = map;
        SuccessType = successType;
    }

    public Structure Target { get; }

    public VariableMapping Mapping { get; }

    public SuccessType SuccessType { get; }
}

public enum SuccessType
{
    DeterministicSuccess, NonDeterministicSuccess, NoMatch 
};