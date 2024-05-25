using Asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Util.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_interpreter_lib.Preprocessing.OLONDetection;

public class OLONRulesFilterer(ILogger logger)
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    private readonly CallGraphBuilder _callGraphBuilder = new CallGraphBuilder();

    private readonly CallGraphCycleFinder _cycleFinder = new CallGraphCycleFinder(logger);
    
    public List<Statement> FilterOlonRules(List<Statement> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        _logger.LogInfo("Filtering OLON rules...");

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

        filteredStatements.ForEach(s => _logger.LogDebug(s.ToString()));
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
