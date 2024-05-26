// <copyright file="DBUnificationResult.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

public class DBUnificationResult
{
    public DBUnificationResult(IEnumerable<Structure> renamedClause, VariableMapping newMapping, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(renamedClause, nameof(renamedClause));
        ArgumentNullException.ThrowIfNull(newMapping, nameof(newMapping));

        RenamedClause = renamedClause;
        NewMapping = newMapping;
        NextInternalIndex = nextInternalIndex;
    }

    public IEnumerable<Structure> RenamedClause { get; }

    public VariableMapping NewMapping { get; }

    public int NextInternalIndex { get; }
}
