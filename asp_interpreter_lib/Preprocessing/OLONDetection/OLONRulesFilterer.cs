namespace Asp_interpreter_lib.Preprocessing.OLONDetection
{
    using Asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Util.ErrorHandling;

    public class OLONRulesFilterer
    {
        private readonly ILogger logger;

        private readonly CallGraphBuilder callGraphBuilder;

        private readonly CallGraphCycleFinder cycleFinder;

        public OLONRulesFilterer(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            this.logger = logger;
            this.callGraphBuilder = new CallGraphBuilder();
            this.cycleFinder = new CallGraphCycleFinder(logger);
        }

        public List<Statement> FilterOlonRules(List<Statement> rules)
        {
            ArgumentNullException.ThrowIfNull(rules, nameof(rules));
            logger.LogInfo("Filtering OLON rules...");

            List<Statement> filteredStatements = new List<Statement>();

            // add all rules without a head.
            foreach (var rule in rules)
            {
                if (!rule.HasHead)
                {
                    filteredStatements.Add(rule);
                }
            }

            var callGraph = callGraphBuilder.BuildCallGraph(rules);
            var statementToCyclesMapping = cycleFinder.FindAllCycles(callGraph);

            // add statement if it any of its cycles are OLON.
            foreach (var mapping in statementToCyclesMapping)
            {
                if (mapping.Value.Any((cycle) => CountNegations(cycle) % 2 != 0))
                {
                    filteredStatements.Add(mapping.Key);
                }
            }

            filteredStatements.ForEach(s => logger.LogDebug(s.ToString()));
            return filteredStatements;
        }

        private int CountNegations(List<CallGraphEdge> cycle)
        {
            int count = 0;

            foreach (var edge in cycle)
            {
                if (edge.TransitionLiteral.HasNafNegation)
                {
                    count += 1;
                }
            }

            return count;
        }
    }

}