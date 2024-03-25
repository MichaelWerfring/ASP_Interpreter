using asp_interpreter_lib.Types;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.CallGraph;

public class CallGraphBuilder
{
    /// <summary>
    /// Build a callgraph from the rules of an ASP program. Ignores rules without a head.
    /// </summary>
    /// <param name="program"></param>
    /// <returns>An directed graph of statements, with edges indicating negation.</returns>
    public AdjacencyGraph<Statement, CallGraphEdge<Statement>> BuildCallGraph(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program, "Must not be null.");

        var graph = new AdjacencyGraph<Statement, CallGraphEdge<Statement>>(false);

        foreach(var statement in program.Statements)
        {
            if(statement.Head.HasValue)
            {
                graph.AddVertex(statement);
            }     
        }

        foreach(var statement in program.Statements)
        {
            foreach(var nafLiteral in statement.Body.Literals)
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
    private List<CallGraphEdge<Statement>> GetEdges(Statement statement, NafLiteral nafLiteral, AdjacencyGraph<Statement, CallGraphEdge<Statement>> graph) 
    {
        if(!nafLiteral.IsClassicalLiteral) 
        {
            throw new ArgumentException($"{nameof(nafLiteral)} must be a classical literal");
        }

        ClassicalLiteral classicalLiteral = nafLiteral.ClassicalLiteral;

        List<Statement> matchingStatements = GetMatchingStatements(classicalLiteral, graph);

        List<CallGraphEdge<Statement>> edges = new List<CallGraphEdge<Statement>>();

        foreach( var currentStatement in matchingStatements)
        {
            edges.Add(new CallGraphEdge<Statement>(statement, currentStatement, nafLiteral.IsNafNegated));
        }

        return edges;
    }

    /// <summary>
    /// Based on a classical literal, get all matching statements(where statement head's functor, arity and classical negation are equal).
    /// </summary>
    /// <param name="literal"></param>
    /// <param name="graph"></param>
    /// <returns>A list of matching statements.</returns>
    private List<Statement> GetMatchingStatements(ClassicalLiteral literal, AdjacencyGraph<Statement, CallGraphEdge<Statement>> graph)
    {
        var matches = new List<Statement>();

        foreach (Statement statement in graph.Vertices)
        {
            Head currentHead = statement.Head;

            ClassicalLiteral? currentLiteral = currentHead.Literal;

            if(currentLiteral == null) 
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
            if(literal.Terms.Count != currentLiteral.Terms.Count)
            {
                continue;
            }
 
            matches.Add(statement);
        }

        return matches;
    }
}

