﻿using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util;
using System.Text;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class GoalSolution
{
    public GoalSolution
    (
        CoinductiveHypothesisSet resultSet,
        VariableMapping resultMapping,
        CallStack stack,
        int nextInternalVariableIndex
    )
    {
        ArgumentNullException.ThrowIfNull(resultSet, nameof(resultSet));
        ArgumentNullException.ThrowIfNull(resultMapping, nameof(resultMapping));
        ArgumentNullException.ThrowIfNull(stack, nameof(stack));

        ResultSet = resultSet;
        ResultMapping = resultMapping;
        NextInternalVariable = nextInternalVariableIndex;
        Stack = stack;
    }

    public CoinductiveHypothesisSet ResultSet { get; }

    public VariableMapping ResultMapping { get; }

    public CallStack Stack { get; }

    public int NextInternalVariable { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("GoalSolution:");
        sb.AppendLine("Resultset:");
        sb.AppendLine($"{{ {ResultSet.Entries.ToList().ListToString()} }}");

        sb.AppendLine("Resultmapping:");
        foreach (var pair in ResultMapping)
        {
            sb.AppendLine($"{pair.Key.ToString()} : {pair.Value.ToString()}");
        }
        sb.AppendLine("Next variable index:");
        sb.AppendLine(NextInternalVariable.ToString());

        sb.AppendLine();

        return sb.ToString();
    }
}