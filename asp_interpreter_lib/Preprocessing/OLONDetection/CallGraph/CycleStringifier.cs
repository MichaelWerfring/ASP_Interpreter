using System.Text;

namespace asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;

public static class CycleStringifier
{
    /// <summary>
    /// A more readable ToString()-Method for a list of edges.
    /// </summary>
    /// <param name="cycle"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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
