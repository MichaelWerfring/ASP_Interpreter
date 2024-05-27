// <copyright file="CallGraphCycleFinder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;

using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Util.ErrorHandling;
using QuikGraph;

/// <summary>
/// A class for finding simple cycles in a call graph,
/// in the sense that no rule head must be passed through more than once.
/// </summary>
public class CallGraphCycleFinder(ILogger logger)
{
    private readonly ILogger logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    /// <summary>
    /// Finds all cycles in the graph.
    /// </summary>
    /// <param name="graph">The input graph.</param>
    /// <returns>A mapping of statements to their respective cycles.</returns>
    /// <exception cref="ArgumentNullException">Thrown if graph is null.</exception>
    public Dictionary<Statement, List<List<CallGraphEdge>>> FindAllCycles(AdjacencyGraph<Statement, CallGraphEdge> graph)
    {
        ArgumentNullException.ThrowIfNull(graph);
        this.logger.LogTrace("Looking for cyles in graph...");

        var vertexToCycleMapping = new Dictionary<Statement, List<List<CallGraphEdge>>>();

        foreach (Statement vertex in graph.Vertices)
        {
            var cycles = this.FindCyclesInvolvingVertex(vertex, graph);

            vertexToCycleMapping.Add(vertex, cycles);
        }

        return vertexToCycleMapping;
    }

    private List<List<CallGraphEdge>> FindCyclesInvolvingVertex(Statement vertex, AdjacencyGraph<Statement, CallGraphEdge> graph)
    {
        List<List<CallGraphEdge>> cycesList = [];

        this.FindCycle(vertex, [], graph, cycesList);

        return cycesList;
    }

    /// <summary>
    /// Checks if two heads are equal (same identifier, same arity).
    /// </summary>
    private bool HaveSameHead(Statement a, Statement b)
    {
        var aHead = a.Head.GetValueOrThrow("Head must be present");
        var bHead = b.Head.GetValueOrThrow("Head must be present");

        if (aHead.Identifier != bHead.Identifier)
        {
            return false;
        }

        if (aHead.HasStrongNegation != bHead.HasStrongNegation)
        {
            return false;
        }

        if (aHead.Terms.Count != bHead.Terms.Count)
        {
            return false;
        }

        return true;
    }

    private void FindCycle(
        Statement vertex,
        List<CallGraphEdge> accumulatedEdges,
        AdjacencyGraph<Statement, CallGraphEdge> graph,
        List<List<CallGraphEdge>> cyclesList)
    {
        if (accumulatedEdges.Any((edge) => this.HaveSameHead(vertex, edge.Source)))
        {
            this.CheckIfCycleInvolvingStart(accumulatedEdges, cyclesList);
            return;
        }

        IEnumerable<CallGraphEdge> edges;

        bool found = graph.TryGetOutEdges(vertex, out edges);
        if (!found)
        {
            return;
        }

        foreach (var edge in edges)
        {
            var copiedList = accumulatedEdges.ToList();

            copiedList.Add(edge);

            this.FindCycle(edge.Target, copiedList, graph, cyclesList);
        }
    }

    private void CheckIfCycleInvolvingStart(List<CallGraphEdge> accumulatedEdges, List<List<CallGraphEdge>> cyclesList)
    {
        if (accumulatedEdges[0].Source.Equals(accumulatedEdges[accumulatedEdges.Count - 1].Target))
        {
            cyclesList.Add(accumulatedEdges);
        }
    }
}