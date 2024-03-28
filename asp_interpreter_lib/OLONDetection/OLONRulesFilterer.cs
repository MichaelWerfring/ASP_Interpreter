using asp_interpreter_lib.OLONDetection.CallGraph;
using asp_interpreter_lib.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.OLONDetection
{
    public class OLONRulesFilterer
    {
        public List<Statement> FilterOlonRules(List<Statement> rules)
        {
            ArgumentNullException.ThrowIfNull(rules, nameof(rules));

            List<Statement> filteredStatements = new List<Statement>();

            // add all rules without a head.
            foreach (var rule in rules)
            {
                if (!rule.HasHead)
                {
                    filteredStatements.Add(rule);
                }
            }

            var callGraph = _callGraphBuilder.BuildCallGraph(rules);
            var statementToCyclesMapping = _cycleFinder.FindAllCycles(callGraph);

            // add statement if it any of its cycles are OLON.
            foreach (var mapping in statementToCyclesMapping)
            {
                if (mapping.Value.Any((cycle) => CountNegations(cycle) % 2 != 0))
                {
                    filteredStatements.Add(mapping.Key);
                }
            }

            return filteredStatements;
        }

        private CallGraphBuilder _callGraphBuilder = new CallGraphBuilder();

        private CallGraphCycleFinder _cycleFinder = new CallGraphCycleFinder();

        private int CountNegations(List<CallGraphEdge> cycle)
        {
            int count = 0;

            foreach(var edge in cycle)
            {
                if(edge.TransitionLiteral.IsNafNegated)
                {
                    count+=1;
                }
            }

            return count;
        }
    }
}
