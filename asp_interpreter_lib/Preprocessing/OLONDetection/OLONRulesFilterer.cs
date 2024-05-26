// <copyright file="OLONRulesFilterer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Preprocessing.OLONDetection;

using Asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for filtering olon rules.
/// </summary>
/// <param name="logger">A logger.</param>
/// <exception cref="ArgumentNullException">Thrown if logger is null.</exception>
public class OLONRulesFilterer(ILogger logger)
{
    private readonly ILogger logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    private readonly CallGraphBuilder callgraphBuilder = new CallGraphBuilder();

    private readonly CallGraphCycleFinder cycleFinder = new CallGraphCycleFinder(logger);

    /// <summary>
    /// Filters olon rules based on a list of input rules.
    /// </summary>
    /// <param name="rules">The input rules.</param>
    /// <returns>A list of statements that are olon rules.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="rules"/> is null.</exception>
    public List<Statement> FilterOlonRules(List<Statement> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        this.logger.LogInfo("Filtering OLON rules...");

        List<Statement> filteredStatements = new List<Statement>();

        // add all rules without a head.
        foreach (var rule in rules)
        {
            if (!rule.HasHead)
            {
                filteredStatements.Add(rule);
            }
        }

        var callGraph = this.callgraphBuilder.BuildCallGraph(rules);
        var statementToCyclesMapping = this.cycleFinder.FindAllCycles(callGraph);

        // add statement if it any of its cycles are OLON.
        foreach (var mapping in statementToCyclesMapping)
        {
            if (mapping.Value.Any((cycle) => this.CountNegations(cycle) % 2 != 0))
            {
                filteredStatements.Add(mapping.Key);
            }
        }

        filteredStatements.ForEach(s => this.logger.LogDebug(s.ToString()));
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