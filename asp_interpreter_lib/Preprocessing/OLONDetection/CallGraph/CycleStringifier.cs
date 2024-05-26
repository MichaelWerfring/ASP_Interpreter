// <copyright file="CycleStringifier.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;

using System.Text;

/// <summary>
/// A class for converting a list of callgraph edges representating a cycle to a string representation.
/// </summary>
public static class CycleStringifier
{
    /// <summary>
    /// A more readable ToString()-Method for a list of edges.
    /// </summary>
    /// <param name="cycle">The cycle.</param>
    /// <returns>A string representation of the cycle list.</returns>
    /// <exception cref="ArgumentNullException">Thrown if cycle is null.</exception>
    public static string CycleToString(List<CallGraphEdge> cycle)
    {
        ArgumentNullException.ThrowIfNull(cycle);

        if (cycle.Count == 0)
        {
            throw new ArgumentException(nameof(cycle));
        }

        if (cycle[0].Source != cycle[cycle.Count - 1].Target)
        {
            throw new ArgumentException(nameof(cycle));
        }

        var stringBuilder = new StringBuilder();

        foreach (var edge in cycle)
        {
            stringBuilder.Append("{ ");
            stringBuilder.Append(edge.Source);
            stringBuilder.Append(" }");
            stringBuilder.Append($" -> {edge.TransitionLiteral} -> ");
        }

        stringBuilder.Append("{ ");
        stringBuilder.Append(cycle[0].Source);
        stringBuilder.Append(" }");

        return stringBuilder.ToString();
    }
}