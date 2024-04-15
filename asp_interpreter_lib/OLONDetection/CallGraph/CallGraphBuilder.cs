using asp_interpreter_lib.Types;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.OLONDetection.CallGraph;

public class CallGraphBuilder
{
    /// <summary>
    /// Build a callgraph from the rules of an ASP program. Ignores rules without a head.
    /// </summary>
    /// <param name="program"></param>
    /// <returns>An directed graph of statements, with edges indicating negation.</returns>
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
            foreach (var nafLiteral in statement.Body.Literals)
            {
                var nafLiteralEdges = GetEdges(statement, nafLiteral, graph);

                graph.AddEdgeRange(nafLiteralEdges);
            }
        }

        return graph;
    }

    /// <summary>
    /// Based on a statement and a naf literal(from that statement's body), create all the necessary edges based on the given graph.
    /// </summary>
    /// <param name="statement"></param>
    /// <param name="nafLiteral"></param>
    /// <param name="graph"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private List<CallGraphEdge> GetEdges(Statement statement, NafLiteral nafLiteral, AdjacencyGraph<Statement, CallGraphEdge> graph)
    {
        if (!nafLiteral.IsClassicalLiteral)
        {
            return new List<CallGraphEdge>();
        }

        ClassicalLiteral classicalLiteral = nafLiteral.ClassicalLiteral;

        List<Statement> matchingStatements = GetMatchingStatements(classicalLiteral, graph);

        List<CallGraphEdge> edges = new List<CallGraphEdge>();

        foreach (var currentStatement in matchingStatements)
        {
            edges.Add(new CallGraphEdge(statement, currentStatement, nafLiteral));
        }

        return edges;
    }

    /// <summary>
    /// Based on a classical literal, get all matching statements(where statement head's functor, arity and classical negation are equal).
    /// </summary>
    /// <param name="literal"></param>
    /// <param name="graph"></param>
    /// <returns>A list of matching statements.</returns>
    private List<Statement> GetMatchingStatements(ClassicalLiteral literal, AdjacencyGraph<Statement, CallGraphEdge> graph)
    {
        var matches = new List<Statement>();

        foreach (Statement statement in graph.Vertices)
        {
            Head currentHead = statement.Head;

            ClassicalLiteral? currentLiteral = currentHead.Literal;

            if (currentLiteral == null)
            {
                continue;
            }

            if (literal.Negated != currentLiteral.Negated)
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