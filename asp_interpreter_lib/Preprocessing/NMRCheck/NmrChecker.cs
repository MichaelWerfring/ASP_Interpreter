﻿using System.ComponentModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Solving.NMRCheck;

public class NmrChecker(PrefixOptions options, ILogger logger)
{
    private readonly PrefixOptions _options = options ?? 
        throw new ArgumentNullException(nameof(options), "The given argument must not be null!");

    private readonly ILogger _logger = logger ?? 
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    private static readonly GoalToLiteralConverter _goalToLiteralConverter = new();

    private readonly DualRuleConverter _converter = new DualRuleConverter(options, logger.GetDummy(), true);

    private List<Statement> GetDualsForCheck(List<Statement> statements)
    {
        List<Statement> duals = [];

        var withoutAnonymous = statements.Select(_converter.Replacer.Replace);
        var headComputed = withoutAnonymous.Select(_converter.ComputeHead).ToList();

        foreach (var statement in statements)
        {
            var head = statement.Head.GetValueOrThrow("Constraint rules must be given a head!");
            var kv = new KeyValuePair<(string, int, bool), List<Statement>>((head.Identifier, head.Terms.Count, head.HasStrongNegation), [statement]);
            duals.AddRange(_converter.ToConjunction(kv));
        }

        duals.ForEach(d => _logger.LogDebug(d.ToString()));

        return duals;
    } 

    public List<Statement> GetSubCheckRules(List<Statement> olonRules, bool notAsName = true)
    {
        ArgumentNullException.ThrowIfNull(olonRules);

        _logger.LogInfo("Generating NMR check...");
        if (olonRules.Count == 0)
        {
            _logger.LogDebug("Finished generation because no OLON rules found in program.");
            var emptyCheck = new Statement();
            emptyCheck.AddHead(new Literal("_nmr_check", false, false, []));
            return [emptyCheck];
        }
        
        // 1) append negation of OLON Rule to its body (If not already present)
        List<Statement> preprocessedRules = PreprocessRules(olonRules);
        
        // 2) generate dual for modified rules
        var tempOlonRules =
            preprocessedRules.Select(r => r.Accept(new StatementCopyVisitor()).GetValueOrThrow()).ToList();

        List<Statement> duals = [];
        // 3) assign unique head (e.g. chk0) 
        duals = GetDualsForCheck(olonRules.ToList());
        AddMissingPrefixes(duals, "_");
        
        Statement nmrCheck = GetCheckRule(tempOlonRules, notAsName);
        AddForallToCheck(nmrCheck);
        
        duals.Insert(0, nmrCheck);

        _logger.LogDebug("NMR check for programm: ");
        duals.ForEach(d => _logger.LogDebug(d.ToString()));

        return duals;
    }

    private void AddMissingPrefixes(List<Statement> duals, string prefix)
    {
        ArgumentNullException.ThrowIfNull(prefix);
        ArgumentNullException.ThrowIfNull(duals);

        foreach (var dual in duals)
        {
            //At this point in the program no rule should be headless
            var head = dual.Head.GetValueOrThrow();

            if (head.Identifier == "not")
            {
                if (head.Terms.Count != 1)
                {
                    throw new InvalidOperationException("Expected exactly one term in the not literal");
                }
                
                var basicTerm = head.Terms[0].Accept(new TermToBasicTermConverter()).GetValueOrThrow();

                if (!basicTerm.Identifier.StartsWith(prefix))
                {
                    basicTerm.Identifier = prefix + basicTerm.Identifier;
                }
            }
            else
            {
                if (!head.Identifier.StartsWith(prefix))
                {
                    head.Identifier = prefix + head.Identifier;
                }
            }
        }
    }

    private Statement GetCheckRule(IEnumerable<Statement> olonRules, bool notAsName = true)
    {
        Statement nmrCheck = new();
        nmrCheck.AddHead(new Literal("_nmr_check", false, false, []));

        // 4) add modified duals to the NMR check goal if it is not already in there
        var nmrBody = new List<Goal>();
        foreach (var rule in olonRules)
        {
            var head = rule.Head.GetValueOrThrow();
            head.HasNafNegation = !notAsName;
            nmrBody.Add(DualRuleConverter.WrapInNot(head));
        }
        
        nmrCheck.AddBody(nmrBody);
        return nmrCheck;
    }

    private List<Statement> PreprocessRules(List<Statement> olonRules)
    {
        ArgumentNullException.ThrowIfNull(olonRules);
        
        if (olonRules.Count == 0)
        {
            return olonRules;
        }

        // 1) append negation of OLON Rule to its body (If not already present)
        //int emptyHeadCount = 0;
        //foreach (var rule in olonRules)
        //{
        //    if (!rule.HasHead)
        //    {
        //        string name = _options.EmptyHeadPrefix + emptyHeadCount++;
        //        rule.AddHead(new Literal(name, false, false, []));
        //        continue;
        //    }
        //    
        //    var head = rule.Head.GetValueOrThrow("Could not parse head!");
        //
        //    var negatedHead = head.Accept(new LiteralCopyVisitor(
        //        new TermCopyVisitor())).GetValueOrThrow("Could not parse negated head!");
        //    negatedHead.HasNafNegation = !negatedHead.HasNafNegation;
        //    
        //    bool containsHead = rule.Body.Find(b => b.ToString() == negatedHead.ToString()) != null;
        //
        //    if (!containsHead)
        //    {
        //        rule.Body.Add(negatedHead);
        //    }
        //}

        for (int i = 0; i < olonRules.Count; i++)
        {
            Statement rule = olonRules[i];
            if (!rule.HasHead)
            {
                string name = "_" +_options.CheckPrefix + (i+1) + "_";
                rule.AddHead(new Literal(name, false, false, []));
                continue;
            }
            
            var head = rule.Head.GetValueOrThrow("Could not parse head!");
        
            var negatedHead = head.Accept(new LiteralCopyVisitor(
                new TermCopyVisitor())).GetValueOrThrow("Could not parse negated head!");
            negatedHead.HasNafNegation = !negatedHead.HasNafNegation;
            
            bool containsHead = rule.Body.Find(b => b.ToString() == negatedHead.ToString()) != null;
        
            if (!containsHead)
            {
                rule.Body.Add(negatedHead);
            }

            head.Identifier = "_" + _options.CheckPrefix + (i + 1) + "_";
        }

            return olonRules;
    }

    private static void AddForallToCheck(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        
        VariableFinder variableFinder = new();

        for (var i = 0; i < statement.Body.Count; i++)
        {
            var literal = statement.Body[i].Accept(_goalToLiteralConverter);
            
            if (!literal.HasValue)
            {
                continue;
            }

            var innerGoal = literal.GetValueOrThrow();

            var variablesInGoal = innerGoal.Accept(variableFinder).GetValueOrThrow().Select(v => v.Identifier).ToList();

            if (variablesInGoal.Count == 0)
            {
                continue;
            }
            
            var forall = DualRuleConverter.NestForall(variablesInGoal, innerGoal);
            statement.Body[i] = forall;
        }
    }
}