﻿using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;

namespace asp_interpreter_lib.Solving.NMRCheck;

public class NmrChecker(PrefixOptions options)
{
    private readonly PrefixOptions _options = options;
    
    private static GoalToLiteralConverter _goalToLiteralConverter = new();

    public List<Statement> GetSubCheckRules(List<Statement> olonRules)
    {
        DualRuleConverter converter = new DualRuleConverter(new AspProgram(olonRules,
                new Query(new Literal("check_converter",
                    false,
                    false,
                    []))),
            _options, true);

        //This will be important later to distingusih between
        //goals generated by the dual rule converter and olon rules
        List<Literal> nonGeneratedHeads = [];
        HashSet<string>variables = new();
        
        // 1) append negation of OLON Rule to its body (If not already present)
        int emptyHeadCount = 0;
        foreach (var rule in olonRules)
        {
            if (!rule.HasHead)
            {
                string name = _options.EmptyHeadPrefix + emptyHeadCount++;
                rule.AddHead(new Literal(name, false, false, []));
                nonGeneratedHeads.Add(rule.Head.GetValueOrThrow());
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
            
            nonGeneratedHeads.Add(head);
        }
        
        // 2) generate dual for modified rule
        var duals = converter.GetDualRules(olonRules);
        
        List<Literal> nmrCheckBody = [];
        // 3) assign unique head (e.g. chk0)
        for (var i = 0; i < duals.Count; i++)
        {
            var rule = duals[i];
            var head = rule.Head.GetValueOrThrow("Could not parse head!");
            
            //head.HasNafNegation = false;
            
            Statement newStatement = new();
            newStatement.AddHead(head);
            newStatement.AddBody(rule.Body);
            duals[i] = newStatement;

            // 4) add modified duals to the NMR check goal 
            // if it is not already in there
            if (nmrCheckBody.Find(b => b.ToString() == head.ToString()) == null&&
                nonGeneratedHeads.Find(b => b.Identifier == head.Identifier 
                && b.Terms.Count == head.Terms.Count) != null)
            {
                head.Identifier = _options.CheckPrefix + head.Identifier;
                nmrCheckBody.Add(head);
            }
        }

        Statement nmrCheck = new();
        nmrCheck.AddHead(new Literal("nmr_check", false, false, []));
        nmrCheck.AddBody([]);
        nmrCheck.Body.AddRange(nmrCheckBody);
        
        //just for readability add nmr_check to the beginning
        duals.Insert(0, nmrCheck);
        
        //Quantify variable in nmr_check
        AddForallToCheck(nmrCheck);
        
        return duals;
    }

    private static void AddForallToCheck(Statement statement)
    {
        //Variable are body variables implicitly
        List<string> variables = [];
        VariableFinder variableFinder = new();
        foreach (var goal in statement.Body)
        {
            variables.AddRange(goal.Accept(variableFinder).
                GetValueOrThrow("Cannot retrieve variables from body!").
                Select(v => v.Identifier));
        }

        for (var i = 0; i < statement.Body.Count; i++)
        {
            var literal = statement.Body[i].Accept(_goalToLiteralConverter);
            
            if (!literal.HasValue)
            {
                continue;
            }

            var innerGoal = literal.GetValueOrThrow();

            var variablesInGoal = innerGoal.Accept(variableFinder).GetValueOrThrow();

            if (variablesInGoal.Count == 0)
            {
                continue;
            }
            
            var forall = DualRuleConverter.NestForall(variables.ToList(), innerGoal);
            statement.Body[i] = forall;
        }
    }
}