using asp_interpreter_lib.ListExtensions;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.OLONDetection.CallGraph
{
    public class CycleFinder<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        public Dictionary<TVertex, List<List<TEdge>>> FindAllCycles(AdjacencyGraph<TVertex, TEdge> graph)
        {
            ArgumentNullException.ThrowIfNull(graph);

            var vertexToCycleMapping = new Dictionary<TVertex, List<List<TEdge>>>();

            foreach (TVertex vertex in graph.Vertices)
            {
                var cycles = FindCyclesInvolvingVertex(vertex, graph);

                vertexToCycleMapping.Add(vertex, cycles);
            }

            return vertexToCycleMapping;
        }

        public List<List<TEdge>> FindCyclesInvolvingVertex(TVertex vertex, AdjacencyGraph<TVertex, TEdge> graph)
        {
            if (vertex == null) throw new ArgumentNullException(nameof(vertex));

            if (graph == null) throw new ArgumentNullException(nameof(graph));

            if (!graph.ContainsVertex(vertex)) throw new ArgumentException(nameof(vertex));


            List<List<TEdge>> cycesList = new List<List<TEdge>>();

            FindCycle(vertex, new List<TEdge>(), graph, cycesList);

            return cycesList;
        }

        private void FindCycle(TVertex vertex, List<TEdge> accumulatedEdges, AdjacencyGraph<TVertex, TEdge> graph, List<List<TEdge>> cyclesList)
        {
            if (accumulatedEdges.Any((edge) => edge.Source.Equals(vertex)))
            {
                CheckIfCycleInvolvingStart(accumulatedEdges, cyclesList);
                return;
            }

            IEnumerable<TEdge> edges;

            bool found = graph.TryGetOutEdges(vertex, out edges);
            if (!found) { return; }

            foreach (var edge in edges)
            {
                var copiedList = accumulatedEdges.ToList();

                copiedList.Add(edge);

                FindCycle(edge.Target, copiedList, graph, cyclesList);
            }
        }

        private void CheckIfCycleInvolvingStart(List<TEdge> accumulatedEdges, List<List<TEdge>> cyclesList)
        {
            if (accumulatedEdges[0].Source!.Equals(accumulatedEdges[accumulatedEdges.Count - 1].Target))
            {
                cyclesList.Add(accumulatedEdges);
            }
        }
    }
}
