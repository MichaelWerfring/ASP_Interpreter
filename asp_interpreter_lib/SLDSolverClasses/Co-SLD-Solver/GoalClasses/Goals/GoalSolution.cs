// <copyright file="GoalSolution.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Text;

/// <summary>
/// Represents a solution to a goal.
/// </summary>
public class GoalSolution
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GoalSolution"/> class.
    /// </summary>
    /// <param name="resultSet">The result chs.</param>
    /// <param name="resultMapping">The result mapping.</param>
    /// <param name="stack">The result callstack.</param>
    /// <param name="nextInternalVariableIndex">The next internal variable index.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="resultSet"/> is null,
    /// ..<paramref name="resultMapping"/> is null,
    /// ..<paramref name="stack"/> is null.</exception>
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

    /// <summary>
    /// Gets the result chs.
    /// </summary>
    public CoinductiveHypothesisSet ResultSet { get; }

    /// <summary>
    /// Gets the result mapping.
    /// </summary>
    public VariableMapping ResultMapping { get; }

    /// <summary>
    /// Gets the result callstack.
    /// </summary>
    public CallStack Stack { get; }

    /// <summary>
    /// Gets the next internal variable index.
    /// </summary>
    public int NextInternalVariable { get; }

    /// <summary>
    /// Converts the solution to a string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
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