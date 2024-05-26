//-----------------------------------------------------------------------
// <copyright file="CallGraphEdge.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;

using Asp_interpreter_lib.Types;
using QuikGraph;
using System.Text;

/// <summary>
/// Represents an edge in a call graph.
/// </summary>
public class CallGraphEdge : IEdge<Statement>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CallGraphEdge"/> class.
    /// </summary>
    /// <param name="source">The source statement.</param>
    /// <param name="target">The target statement.</param>
    /// <param name="transitionLiteral">The transition literal.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. source is null.
    /// .. target is null.
    /// .. transition literal is null.</exception>
    public CallGraphEdge(
        Statement source,
        Statement target,
        Literal transitionLiteral)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(transitionLiteral);

        this.Source = source;
        this.Target = target;
        this.TransitionLiteral = transitionLiteral;
    }

    /// <summary>
    /// Gets the source statement.
    /// </summary>
    public Statement Source { get; }

    /// <summary>
    /// Gets the transition literal.
    /// </summary>
    public Literal TransitionLiteral { get; }

    /// <summary>
    /// Gets the target statement.
    /// </summary>
    public Statement Target { get; }

    /// <summary>
    /// Converts the edge to a string representation.
    /// </summary>
    /// <returns>A string representation.</returns>
    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append($"{{{this.Source}}} ");
        builder.Append($"-> {this.TransitionLiteral} ->");
        builder.Append($"{{{this.Target}}}");
        return builder.ToString();
    }
}