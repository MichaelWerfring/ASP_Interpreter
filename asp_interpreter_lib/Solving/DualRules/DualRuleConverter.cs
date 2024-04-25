using System.Collections;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using Antlr4.Runtime.Tree.Xpath;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;
using BasicTerm = asp_interpreter_lib.Types.Terms.BasicTerm;
using GoalNegator = asp_interpreter_lib.Solving.DualRules.GoalNegator;
using VariableTerm = asp_interpreter_lib.Types.Terms.VariableTerm;

namespace asp_interpreter_lib.Solving;

public class DualRuleConverter
{
    private readonly VariableFinder _variableFinder;
    
    private readonly PrefixOptions _options;

    private bool _negateInnerForall;
    
    public DualRuleConverter(PrefixOptions options,bool negateInnerForall = true)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _negateInnerForall = negateInnerForall;
        _options = options;
        _variableFinder = new VariableFinder();
        _negateInnerForall = negateInnerForall;
    }

    public Statement ComputeHead(Statement rule)
    {
        HeadRewriter rewriter = new HeadRewriter(_options, rule);
        return rewriter.Visit(rule).GetValueOrThrow("Cannot rewrite head of rule!");
    }

    private Dictionary<(string,int, bool), List<Statement>> PreprocessRules(IEnumerable<Statement> rules)
    {
        //heads mapped to all bodies occuring in the program
        Dictionary<(string, int, bool), List<Statement>> disjunctions = [];
        
        foreach (var rule in rules)
        {
            //Headless rules will be treated within nmr check
            if (!rule.HasHead) continue;
            var head = rule.Head.GetValueOrThrow("Headless rules must be treated by the NMR check!");
            
            var signature = (head.Identifier, head.Terms.Count, head.HasStrongNegation);
            //var signature = ((head.HasStrongNegation ? "-" : "") + head.Identifier, head.Terms.Count);
            
            var converted = ComputeHead(rule);
            
            if (!disjunctions.TryAdd(signature, [converted]))
            {
                //disjunctions[head].Add(rule);
                disjunctions[signature].Add(converted);
            }
        }
        
        return disjunctions;
    }

    public List<Statement> GetDualRules(IEnumerable<Statement> rules,bool appendPrefix = true)
    {
        List<Statement> duals = [];
        
        var disjunctions = PreprocessRules(rules.Select(ComputeHead));
        foreach (var disjunction in disjunctions)
        {
            duals.AddRange(ToConjunction(disjunction, appendPrefix));
        }

        return duals;
    }

    private IEnumerable<Statement> ToConjunction(
        KeyValuePair<(string, int, bool), List<Statement>> disjunction, 
        bool appendPrefix = true)
    {
        List<Statement> duals = [];

        // 1) generate new rule at top (named like input)
        Statement wrapper = new();
        wrapper.AddHead(new Literal(
            (appendPrefix ? _options.DualPrefix : "") + disjunction.Key.Item1,
            false,
            disjunction.Key.Item3,
            GenerateVariables(disjunction.Key.Item2)
            ));

        //If there is just one statement its not a disjunction
        if (disjunction.Value.Count == 1)
        {
            return ToDisjunction(disjunction.Value[0]);
        }
        
        List<Goal> wrapperBody = [];
        
        for (var i = 0; i < disjunction.Value.Count; i++)
        {
            // 2) rename old rule heads
            var goal = disjunction.Value[i];
            var head = goal.Head.GetValueOrThrow();
            head.Identifier += (i + 1);
            
            // 3) add heads to body of new rule
            
            // 3.1 insert variables from head into body goals
            var copy = goal.Head.GetValueOrThrow().Accept(new LiteralCopyVisitor(new TermCopyVisitor()))
                .GetValueOrThrow("Cannot copy rule!");
            copy.Identifier = (appendPrefix ? _options.DualPrefix : "") + copy.Identifier;
            copy.Terms.Clear();
            copy.Terms.AddRange(wrapper.Head.GetValueOrThrow().Terms);
            
            wrapperBody.Add(copy);
            
            
            // 4) generate duals for old rules
            // 5) add duals to list
            duals.AddRange(ToDisjunction(goal));
        }
        
        wrapper.AddBody(wrapperBody);
        duals.Insert(0, wrapper);
        
        return duals;
    }

    private List<ITerm> GenerateVariables(int number)
    {
        List<ITerm> vars = [];
        for (int i = 0; i < number ; i++)
        {
            vars.Add(new VariableTerm("V" + (i + 1)));
        }

        return vars;
    }
    
    public IEnumerable<Statement> ToDisjunction(Statement rule, bool appendPrefix = true)
    {
        if (!rule.HasHead)
        {
            return [rule];
        }

        List<Statement> duals = [];
        var head = rule.Head.GetValueOrThrow();
        bool forallApplicable = GetBodyVariables(rule).Count != 0;
        
        
        for (var i = 0; i < rule.Body.Count; i++)
        {
            if (forallApplicable)
            {
                duals.AddRange(AddForall(rule));
                continue;
            }

            var goal = rule.Body[i];
            var dualGoal = GoalNegator.Negate(goal);
            
            var newHead = new Literal(
                 (appendPrefix? "not_" : "") + head.Identifier,
                false,
                head.HasStrongNegation,
                head.Terms);
            
            //Add preceding
            var body = new List<Goal>();
            body.AddRange(rule.Body[0..i]);
            body.Add(dualGoal);
            
            //Add new statement to duals
            var dualStatement = new Statement();
            dualStatement.AddHead(newHead);
            dualStatement.AddBody(body);
            
            duals.Add(dualStatement);
        }

        return duals;
    }
    
    private List<string> GetBodyVariables(Statement rule)
    {
        if (!rule.HasHead && rule.HasBody)
        {
            List<string> variables = [];
            foreach (var goal in rule.Body)
            {
                variables.AddRange(goal.Accept(_variableFinder).
                    GetValueOrThrow("Cannot retrieve variables from body!").
                    Select(v => v.Identifier));
            }
            
            return variables;
        }

        if (rule.HasHead && !rule.HasBody)
        {
            return [];
        }

        if (rule.HasHead && rule.HasBody)
        {
            List<string> bodyVar = [];
            foreach (var goal in rule.Body)
            {
                bodyVar.AddRange(goal.Accept(_variableFinder).
                    GetValueOrThrow("Cannot retrieve variables from body!").
                    Select(v => v.Identifier));
            }

            var headVar = rule.Head.GetValueOrThrow().Accept(_variableFinder).
                GetValueOrThrow("Cannot retrieve variables from head!").Select(v => v.Identifier);
            
            return bodyVar.Except(headVar).ToList();
        }
        
        return [];
    }

    public IEnumerable<Statement> AddForall(Statement rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
     
        var bodyVariables = GetBodyVariables(rule);

        //Headless statements are treated by the NMR Check
        if (!rule.HasHead || bodyVariables.Count == 0)
        {
            return [rule];
        }
        
        //Copy the statement before
        var ruleCopy = rule.Accept(new StatementCopyVisitor()).GetValueOrThrow("Cannot copy rule!");
        var ruleCopyHead = ruleCopy.Head.GetValueOrThrow("Cannot retrieve head from copy!");
        
        // get all variables from body
        var variables = ruleCopy.
            Accept(_variableFinder).
            GetValueOrThrow("Cannot retrieve variables from body!").
            DistinctBy(v => v.Identifier);

        // put all variables form body into head
        ruleCopyHead.Terms = [];
        ruleCopyHead.Identifier = _options.ForallPrefix + ruleCopyHead.Identifier;
        
        foreach (var variable in variables)
        {
            ruleCopyHead.Terms.Add(new VariableTerm(variable.Identifier));
        }
        
        // generate duals normally
        var duals = ToDisjunction(ruleCopy, false).ToList();

        // add forall over the new predicate
        var innerGoal = duals.First().Head.GetValueOrThrow();
        
        //append body with (nested) forall
        rule.Body.Clear();
        rule.Body.AddRange(([NestForall(bodyVariables.ToList(), innerGoal)]));

        // prefix like dual
        rule.Head.GetValueOrThrow().Identifier =_options.DualPrefix + rule.Head.GetValueOrThrow().Identifier; 
        
        duals.Insert(0, rule);
        
        return duals;
    }
    
    public static Goal NestForall(List<string> bodyVariables, Literal innerGoal)
    {
        if (bodyVariables.Count == 0)
        {
            return innerGoal;
        }
        
        string v = bodyVariables[0];
        bodyVariables.RemoveAt(0);

        var result = NestForall(bodyVariables, innerGoal);
        
        //return new BasicTerm("forall", [ new VariableTerm(v), result]);
        var f = new Forall(new VariableTerm(v), result);
        string s = f.ToString();
        return f;
    }
}