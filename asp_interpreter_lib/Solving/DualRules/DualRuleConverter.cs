using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;
using BasicTerm = asp_interpreter_lib.Types.Terms.BasicTerm;
using VariableTerm = asp_interpreter_lib.Types.Terms.VariableTerm;

namespace asp_interpreter_lib.Solving;

public class DualRuleConverter
{
    private readonly HashSet<string> _variables;

    private readonly HashSet<string> _ruleNames;
    
    private readonly List<VariableTerm> _variableTerms;
    
    private readonly VariableTermConverter _variableTermConverter;
    
    private readonly VariableFinder _variableFinder;

    public DualRuleConverter(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);
        _variables = new HashSet<string>();
        _variableTermConverter = new VariableTermConverter();
        _ruleNames = program.Accept(new RuleNameFinder()).
            GetValueOrThrow("Cannot retrieve rule names from program!");
        _variableFinder = new VariableFinder();
        
        var variableGetter = new VariableFinder();
        _variableTerms = program.Accept(variableGetter).
            GetValueOrThrow("Cannot retrieve variables from program!");
        _variableTerms.ForEach(t => _variables.Add(t.Identifier));
    }

    public Statement ComputeHead(Statement rule)
    {
        HeadRewriter rewriter = new HeadRewriter("rwh", rule);
        return rewriter.Visit(rule).GetValueOrThrow("Cannot rewrite head of rule!");
    }

    private Dictionary<(string,int), List<Statement>> PreprocessRules(List<Statement> rules)
    {
        //heads mapped to all bodies occuring in the program
        Dictionary<(string,int), List<Statement>> disjunctions = [];
        
        foreach (var rule in rules)
        {
            //Headless rules will be treated within nmr check
            if (!rule.HasHead) continue;
            var head = rule.Head.GetValueOrThrow("Headless rules must be treated by the NMR check!");
            var signature = (head.Identifier, head.Terms.Count);
            
            var converted = ComputeHead(rule);
            
            if (!disjunctions.TryAdd(signature, [converted]))
            {
                //disjunctions[head].Add(rule);
                disjunctions[signature].Add(converted);
            }
        }

        return disjunctions;
    }
    
    public List<Statement> GetDualRules(List<Statement> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        
        List<Statement> duals = [];
        var disjunctions = PreprocessRules(rules);
        
        foreach (var disjunction in disjunctions)
        {
            var statements = disjunction.Value; 
            if (statements.Count == 1)
            {
                statements[0].Head.
                    GetValueOrThrow("Headless rules must be treated by the NMR check!").HasNafNegation = true;

                var withForall = AddForall(statements[0], _variables);
                if (withForall.Count > 0)
                {
                    duals.AddRange(withForall);
                }
                else
                {
                    duals.AddRange(GetDualRules(statements[0]));
                }
                
                continue;
            }
            
            var newVariable = new VariableTerm(ASPExtensions.GenerateUniqeName("", _ruleNames, "dis"));
            
            var newStatement = new Statement();
            newStatement.AddHead(
                new Literal(disjunction.Key.Item1, true, false, [newVariable]));
            
            //For readability add the statement beforehand
            duals.Add(newStatement);
            List<Goal> newBody = [];

            for (var i = 0; i < statements.Count; i++)
            {
                var statement = statements[i];
                var head = statement.Head
                    .GetValueOrThrow("Headless statements must be treated by the NMR check!");
                
                string tempVariableId =
                    ASPExtensions.GenerateUniqeName(head.Identifier, _ruleNames, "idis");
                
                head.Identifier = tempVariableId;
                
                //newBody.Add(new NafLiteral(new ClassicalLiteral(tempVariableId, false, [newVariable]), true));
                //newBody.Add(new NafLiteral(new ClassicalLiteral(tempVariableId, false, [newVariable]), 
                //   true));
                newBody.Add(new Literal(tempVariableId, true, false, [newVariable]));

                var withForall = AddForall(statement, _variables);
                if (withForall.Count > 0)
                {
                    duals.AddRange(withForall);
                    continue;
                }

                //GetDualRules(ComputeHead(statement)).ForEach(
                GetDualRules(statement).ForEach(
                    s =>
                    {
                        s.Head.IfHasValue(h =>
                        {
                            h.Identifier = tempVariableId;
                            h.HasNafNegation = true;
                        });
                        duals.Add(s);
                    });
            }


            newStatement.AddBody(newBody);
        }

        return duals;
    }
    
    public List<Statement> GetDualRules(Statement stmt)
    {
        ArgumentNullException.ThrowIfNull(stmt);

        var copier = new StatementCopyVisitor();
        Statement rule = stmt.Accept(copier).GetValueOrThrow("Cannot copy given statement!");

        if (!stmt.HasBody)
        {
            //return GetDualRules([ComputeHead(stmt)]);
            return GetDualRules(stmt);
        }
        
        List<Statement> duals = [];

        var negator = new GoalNegator(new LiteralCopyVisitor(new TermCopyVisitor()));

        for (var i = 0; i < rule.Body.Count; i++)
        {
            var dualStatement = new Statement();
            
            //Maybe set negated on the classical literal in the head 
            //to indicate a dual rule
            //rule.Head.IsDual = true;
            var head = rule.Head.GetValueOrThrow("Headless rules must be handled by the NMR check!");
            head.HasNafNegation = true;
            dualStatement.AddHead(head);

            List<Goal> dualBody = [];
            //add preceding
            dualBody.AddRange(rule.Body[0..i]);
            
            //add negated
            var goal = rule.Body[i];
            var negated = goal.Accept(negator).GetValueOrThrow("Cannot negate goal!");
            dualBody.Add(negated);
            
            dualStatement.AddBody(dualBody);
            
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

    public List<Statement> AddForall(Statement statement, HashSet<string> variablesInProgram)
    {
        ArgumentNullException.ThrowIfNull(statement);
        ArgumentNullException.ThrowIfNull(variablesInProgram);
        
        var bodyVariables = GetBodyVariables(statement);

        if (bodyVariables.Count == 0)
        {
            return [];
        }

        //Headless statements are treated by the NMR Check
        if (!statement.HasHead)
        {
            return [statement];
        }
        
        //Copy the statement before
        Statement rule = statement.Accept(new StatementCopyVisitor()).
            GetValueOrThrow("Cannot retrieve copy of statement");
        
        List<Statement> duals = [];

        var head = rule.Head.GetValueOrThrow();
        // 1) Compute Duals Normally but replace predicate in the head
        string newId = 
            ASPExtensions.GenerateUniqeName(head.Identifier, variablesInProgram, "fa");
        string oldId = head.Identifier;
        head.Identifier = newId;
        duals.AddRange(GetDualRules(rule));
        
        
        // 2) Body Variables added to head of each dual
        
        foreach (var variable in bodyVariables)
        {
            //They share the same head
            duals[0].Head.IfHasValue(h => h.Terms.Add(new VariableTerm(variable)));
        }
        
        // 3) Create Clause with forall over the new Predicate
        Statement forall = new();
        
        head.HasNafNegation = true;
        forall.AddHead(head);
        Literal innerGoal;
        //Made Disjunction
        if (duals.Count > 1)
        {
            var dualHead = duals[0].Head.GetValueOrThrow();
            innerGoal = new Literal(
                newId,
                dualHead.HasNafNegation,
                dualHead.HasStrongNegation,
                dualHead.Terms);
        }
        else
        {
            var dualHead = duals[0].Head.GetValueOrThrow();
            innerGoal = new Literal(
                newId,
                !dualHead.HasNafNegation,
                dualHead.HasStrongNegation,
                dualHead.Terms);
        }
        
        //append body with (nested) forall
        forall.AddBody([NestForall(bodyVariables.ToList(), innerGoal)]);
        
        duals.Insert(0,forall);
        head.Identifier = oldId;
        return duals;
    }

    private static Goal NestForall(List<string> bodyVariables, Literal innerGoal)
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