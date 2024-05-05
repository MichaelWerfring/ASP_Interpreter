﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;

public class DisunificationResult
{
    public DisunificationResult(Variable variable, ISimpleTerm term, bool isPositive)
    {
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        Variable = variable;
        Term = term;
        IsPositive = isPositive;
    }

    public Variable Variable { get; }

    public ISimpleTerm Term { get; }

    public bool IsPositive { get; }
}
