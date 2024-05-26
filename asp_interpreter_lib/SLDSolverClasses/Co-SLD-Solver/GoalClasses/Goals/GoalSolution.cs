// <copyright file="GoalSolution.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Text;

public class GoalSolution
{
    public GoalSolution(
        CoinductiveHypothesisSet resultSet,
        VariableMapping resultMapping,
        CallStack stack,
        int nextInternalVariableIndex)
    {
        ArgumentNullException.ThrowIfNull(resultSet, nameof(resultSet));
        ArgumentNullException.ThrowIfNull(resultMapping, nameof(resultMapping));
        ArgumentNullException.ThrowIfNull(stack, nameof(stack));

        this.ResultSet = resultSet;
        this.ResultMapping = resultMapping;
        this.NextInternalVariable = nextInternalVariableIndex;
        this.Stack = stack;
    }

    public CoinductiveHypothesisSet ResultSet { get; }

    public VariableMapping ResultMapping { get; }

    public CallStack Stack { get; }

    public int NextInternalVariable { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Resultset:");
        sb.AppendLine($"{{ {this.ResultSet} }}");

        sb.AppendLine("Resultmapping:");
        sb.AppendLine(this.ResultMapping.ToString());

        sb.AppendLine("Next variable index:");
        sb.AppendLine(this.NextInternalVariable.ToString());

        return sb.ToString();
    }
}