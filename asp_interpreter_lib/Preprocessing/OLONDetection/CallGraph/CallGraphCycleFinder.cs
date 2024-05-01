using asp_interpreter_lib.Types;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph
{
    /// <summary>
    /// A class for finding simple cycles in a call graph,
    /// in the sense that no rule head must be passed through more than once.
    /// </summary>
    public class CallGraphCycleFinder
    {
        public Dictionary<Statement, List<List<CallGraphEdge>>> FindAllCycles(AdjacencyGraph<Statement, CallGraphEdge> graph)
        {
            ArgumentNullException.ThrowIfNull(graph);

            var vertexToCycleMapping = new Dictionary<Statement, List<List<CallGraphEdge>>>();

            foreach (Statement vertex in graph.Vertices)
            {
                var cycles = FindCyclesInvolvingVertex(vertex, graph);

                vertexToCycleMapping.Add(vertex, cycles);
            }

            return vertexToCycleMapping;
        }

        public List<List<CallGraphEdge>> FindCyclesInvolvingVertex(Statement vertex, AdjacencyGraph<Statement, CallGraphEdge> graph)
        {
            if (vertex == null) throw new ArgumentNullException(nameof(vertex));

            if (graph == null) throw new ArgumentNullException(nameof(graph));

            if (!graph.ContainsVertex(vertex)) throw new ArgumentException(nameof(vertex));


            List<List<CallGraphEdge>> cycesList = new List<List<CallGraphEdge>>();

            FindCycle(vertex, new List<CallGraphEdge>(), graph, cycesList);

            return cycesList;
        }

        /// <summary>
        /// Checks if two heads are equal (same identifier, same arity).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private bool HaveSameHead(Statement a, Statement b)
        {
            if (!a.HasHead || !b.HasHead) throw new ArgumentException("Must both have heads");

            if (!a.HasHead || !b.HasHead) throw new ArgumentException("Must both have literals in their heads!");

            var aHead = a.Head.GetValueOrThrow("Head must be present");
            var bHead = b.Head.GetValueOrThrow("Head must be present");

            if (aHead.Identifier != bHead.Identifier) return false;

            if (aHead.Terms.Count != bHead.Terms.Count) return false;

            return true;
        }

        private void FindCycle(Statement vertex, List<CallGraphEdge> accumulatedEdges, AdjacencyGraph<Statement, CallGraphEdge> graph, List<List<CallGraphEdge>> cyclesList)
        {
            if (accumulatedEdges.Any((edge) => HaveSameHead(vertex, edge.Source)))
            {
                CheckIfCycleInvolvingStart(accumulatedEdges, cyclesList);
                return;
            }

            IEnumerable<CallGraphEdge> edges;

            bool found = graph.TryGetOutEdges(vertex, out edges);
            if (!found) { return; }

            foreach (var edge in edges)
            {
                var copiedList = accumulatedEdges.ToList();

                copiedList.Add(edge);

                FindCycle(edge.Target, copiedList, graph, cyclesList);
            }
        }

        private void CheckIfCycleInvolvingStart(List<CallGraphEdge> accumulatedEdges, List<List<CallGraphEdge>> cyclesList)
        {
            if (accumulatedEdges[0].Source!.Equals(accumulatedEdges[accumulatedEdges.Count - 1].Target))
            {
                cyclesList.Add(accumulatedEdges);
            }
        }
    }
}
