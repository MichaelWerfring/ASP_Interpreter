using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;
using asp_interpreter_lib.Util.ErrorHandling;
using VariableTerm = asp_interpreter_lib.Types.Terms.VariableTerm;

namespace asp_interpreter_lib.Solving.DualRules;

public class DualRuleConverter
{
    private readonly VariableFinder _variableFinder;
    
    private readonly PrefixOptions _options;

    private readonly ILogger _logger;
    
    private readonly bool _notAsName;

    public DualRuleConverter(
        PrefixOptions options,
        ILogger logger,
        bool notAsName = true)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _options = options;
        _logger = logger;
        _notAsName = notAsName;
        _variableFinder = new VariableFinder();
    }

    public Statement ComputeHead(Statement rule)
    {
        string oldRule = rule.ToString();
        HeadAtomEliminator rewriter = new(_options, rule);
        var statement = rewriter.Rewrite(rule); 
        _logger.LogTrace("Rewrite head from: " + oldRule + " to: " + statement.ToString());
        return statement;
    }

    private Dictionary<(string,int, bool), List<Statement>> PreprocessRules(IEnumerable<Statement> rules)
    {
        //heads mapped to all bodies occuring in the program
        Dictionary<(string, int, bool), List<Statement>> disjunctions = [];
        _logger.LogTrace("Started preprocessing for dual rules.");

        foreach (var rule in rules)
        {
            //Headless rules will be treated within nmr check
            if (!rule.HasHead) continue;
            var head = rule.Head.GetValueOrThrow("Headless rules must be treated by the NMR check!");
            
            var signature = (head.Identifier, head.Terms.Count, head.HasStrongNegation);
            
            var converted = ComputeHead(rule);
            
            if (!disjunctions.TryAdd(signature, [converted]))
            {
                disjunctions[signature].Add(converted);
                _logger.LogTrace("Disjunction found: " + 
                    signature.HasStrongNegation + 
                    signature.Identifier + "/" + signature.Count);
            }
        }

        _logger.LogTrace("Finished preprocessing for dual rules.");
        return disjunctions;
    }

    private IEnumerable<Statement> GetGoalsOnlyAppearingInBody(List<Statement> rules, bool appendPrefix = true)
    {
        HashSet<(string, int, bool)> heads = [];

        //First find all heads
        foreach (var rule in rules)
        {
            rule.Head.IfHasValue(h => heads.Add(
                (h.Identifier, h.Terms.Count, h.HasStrongNegation)));
        }

        var literalVisitor = new GoalToLiteralConverter();

        List<Statement> statements = [];

        for (int i = 0; i < rules.Count; i++)
        {
            Statement rule = rules[i];

            foreach (var goal in rule.Body)
            {
                var literal = goal.Accept(literalVisitor);
                if (!literal.HasValue) continue;

                var actual = literal.GetValueOrThrow();
                if (heads.Contains(
                    (actual.Identifier, actual.Terms.Count, actual.HasStrongNegation))) continue;
                
                Statement newStatement = new();

                if (_notAsName)
                {
                    newStatement.AddHead(new Literal(
                        (appendPrefix ? _options.DualPrefix : "") + actual.Identifier,
                        false,
                        actual.HasStrongNegation,
                        actual.Terms));
                }
                else
                {
                    newStatement.AddHead(new Literal(
                        actual.Identifier,
                        true,
                        actual.HasStrongNegation,
                        actual.Terms));
                }

                _logger.LogTrace("No Head found for: " + actual.ToString() + " generated: " + newStatement.ToString());
                statements.Add(newStatement);
            }
        }

        return statements;
    }

    public List<Statement> GetDualRules(IEnumerable<Statement> rules,bool appendPrefix = true)
    {
        _logger.LogInfo("Generating dual rules...");

        List<Statement> duals = [];

        var headComputed = rules.Select(ComputeHead).ToList();
        var t = GetGoalsOnlyAppearingInBody(headComputed);
        var disjunctions = PreprocessRules(headComputed);
        foreach (var disjunction in disjunctions)
        {
            duals.AddRange(ToConjunction(disjunction, appendPrefix));
        }

        duals.AddRange(t);

        _logger.LogDebug("The dual rules for program: ");
        duals.ForEach(d => _logger.LogDebug(d.ToString()));

        return duals;
    }

    private IEnumerable<Statement> ToConjunction(
        KeyValuePair<(string, int, bool), List<Statement>> disjunction, 
        bool appendPrefix = true)
    {
        List<Statement> duals = [];

        // 1) generate new rule at top (named like input)
        Statement wrapper = new();

        if (_notAsName)
        {
            wrapper.AddHead(new Literal(
                (appendPrefix ? _options.DualPrefix : "") + disjunction.Key.Item1,
                false,
                disjunction.Key.Item3,
                GenerateVariables(disjunction.Key.Item2)
            ));
        }
        else
        {
            wrapper.AddHead(new Literal(
                disjunction.Key.Item1,
                true,
                disjunction.Key.Item3,
                GenerateVariables(disjunction.Key.Item2)
            ));
        }

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
            
            copy.Terms.Clear();
            copy.Terms.AddRange(wrapper.Head.GetValueOrThrow().Terms);
            
            if (_notAsName)
            {
                copy.Identifier = (appendPrefix ? _options.DualPrefix : "") + copy.Identifier;
            }
            else
            {
                copy.HasNafNegation = true;
            }
            
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
            vars.Add(new VariableTerm(_options.VariablePrefix + (i + 1)));
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

        //If it is a fact, just add the prefix
        if (!rule.HasBody)
        {
            Literal newHead;
            
            if (_notAsName)
            {
                newHead = new Literal(
                    (appendPrefix ? _options.DualPrefix : "") + head.Identifier,
                    false,
                    head.HasStrongNegation,
                    head.Terms);
    
            }
            else
            {
                newHead = new Literal(
                    head.Identifier,
                    true,
                    head.HasStrongNegation,
                    head.Terms);
            }
            
            var newFact = new Statement();
            newFact.AddHead(newHead);
            duals.Add(newFact);
            return duals;
        }

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
            
            Literal newHead;
            if (_notAsName)
            {
                newHead = new Literal(
                    (appendPrefix? _options.DualPrefix : "") + head.Identifier,
                    false,
                    head.HasStrongNegation,
                    head.Terms);
            }
            else
            {
                newHead = new Literal(
                    head.Identifier,
                    true,
                    head.HasStrongNegation,
                    head.Terms);
            }
            
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
        rule.Body.AddRange(([NestForall([.. bodyVariables], innerGoal)]));

        // prefix like dual

        if (_notAsName)
        {
            rule.Head.GetValueOrThrow().Identifier =_options.DualPrefix + rule.Head.GetValueOrThrow().Identifier;
        }
        else
        {
            rule.Head.GetValueOrThrow().HasNafNegation = true;
        }
        
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

        return new Forall(new VariableTerm(v), result);
    }
}