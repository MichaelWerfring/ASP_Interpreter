using asp_interpreter_lib.Types;
using QuikGraph;
using System.Text;

namespace asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;

/// <summary>
/// Represents an edge in a call graph.
/// </summary>
/// <typeparam name="TVertex">The vertex type</typeparam>
public class CallGraphEdge : IEdge<Statement>
{
    public CallGraphEdge(Statement source, Statement target, Literal transitionLiteral)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(transitionLiteral);

        Source = source;
        Target = target;
        TransitionLiteral = transitionLiteral;
    }

    public Statement Source { get; }

    public Literal TransitionLiteral { get; }

    public Statement Target { get; }

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append($"{{{Source}}} ");
        builder.Append($"-> {TransitionLiteral} ->");
        builder.Append($"{{{Target}}}");
        return builder.ToString();
    }
}
