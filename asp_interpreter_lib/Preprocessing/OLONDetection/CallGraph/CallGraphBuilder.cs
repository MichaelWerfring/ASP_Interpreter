// <copyright file="CallGraphBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;

using Asp_interpreter_lib.Types;
using QuikGraph;
using Asp_interpreter_lib.Types.TypeVisitors;

/// <summary>
/// A class that builds a callgraph from a list of statements.
/// </summary>
public class CallGraphBuilder
{
    /// <summary>
    /// Builds a callgraph from the rules of an ASP program. Ignores rules without a head.
    /// </summary>
    /// <param name="statements">The list of statements.</param>
    /// <returns>An directed graph of statements, with edges indicating negation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="statements"/> is null.</exception>
    public AdjacencyGraph<Statement, CallGraphEdge> BuildCallGraph(List<Statement> statements)
    {
        ArgumentNullException.ThrowIfNull(statements, "Must not be null.");

        var graph = new AdjacencyGraph<Statement, CallGraphEdge>();

        foreach (var statement in statements)
        {
            if (statement.Head.HasValue)
            {
                graph.AddVertex(statement);
            }
        }

        foreach (var statement in graph.Vertices)
        {
            foreach (var literal in statement.Body)
            {
                if (literal is not Literal)
                {
                    continue;
                }

                var literalEdges = this.GetEdges(statement, literal, graph);

                graph.AddEdgeRange(literalEdges);
            }
        }

        return graph;
    }

    /// <summary>
    /// Based on a statement and a goal (from that statement's body), create all the necessary edges based on the given graph.
    /// </summary>
    /// <param name="statement">The current statement.</param>
    /// <param name="goal">The current goal from the statement's body.</param>
    /// <param name="graph">The call graph.</param>
    /// <returns>A list of outgoind edges.</returns>
    private List<CallGraphEdge> GetEdges(Statement statement, Goal goal, AdjacencyGraph<Statement, CallGraphEdge> graph)
    {
        var literalConverter = new GoalToLiteralConverter();
        var literal = goal.Accept(literalConverter).
            GetValueOrThrow($"{nameof(goal)} must be a classical literal");

        List<Statement> matchingStatements = this.GetMatchingStatements(literal, graph);

        List<CallGraphEdge> edges = new List<CallGraphEdge>();

        foreach (var currentStatement in matchingStatements)
        {
            edges.Add(new CallGraphEdge(statement, currentStatement, literal));
        }

        return edges;
    }

    /// <summary>
    /// Based on a classical literal, get all matching statements(where statement head's functor, arity and classical negation are equal).
    /// </summary>
    private List<Statement> GetMatchingStatements(Literal literal, AdjacencyGraph<Statement, CallGraphEdge> graph)
    {
        var matches = new List<Statement>();

        foreach (Statement statement in graph.Vertices)
        {
            if (!statement.HasHead)
            {
                continue;
            }

            Literal currentLiteral = statement.Head.GetValueOrThrow();

            if (literal.HasStrongNegation != currentLiteral.HasStrongNegation)
            {
                continue;
            }

            if (literal.Identifier != currentLiteral.Identifier)
            {
                continue;
            }

            if (literal.Terms.Count != currentLiteral.Terms.Count)
            {
                continue;
            }

            matches.Add(statement);
        }

        return matches;
    }
}