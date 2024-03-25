using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.CallGraph;

/// <summary>
/// Represents an edge in a call graph.
/// </summary>
/// <typeparam name="TVertex">The vertex type</typeparam>
public class CallGraphEdge<TVertex> :IEdge<TVertex>
{
    public CallGraphEdge(TVertex source, TVertex target, bool isNegated)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);

        IsNegated = isNegated;
        Source = source;
        Target = target;
    }

    public TVertex Source { get; private set; }

    public TVertex Target { get; private set; }

    public bool IsNegated { get; private set; }
}
