using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
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
        ArgumentNullException.ThrowIfNull(rule);

        
        
        //Perform this recursively
        var terms = rule.Head.Literal?.Terms;
        if (terms == null) return rule;

        var variablesInHead = new HashSet<string>();
        int count = terms.Count;
        
        for (var i = 0; i < count; i++)
        {
            var term = terms[i];
            var variableTerm = term.Accept(_variableTermConverter);

            //If the term is not a variable it is replaced by a variable and unified with it
            if (!variableTerm.HasValue)
            {
                
                //Accept ToString/Null values for now
                var newHeadVariable = new VariableTerm(
                    ASPExtensions.GenerateUniqeName(term.ToString() ?? "", _variables, "rwh"));
                
                //replace head
                terms[terms.IndexOf(term)] = newHeadVariable;
                
                //replace body
                rule.Body.Literals.Insert(0,new NafLiteral(new BinaryOperation(
                    newHeadVariable, new Equality(), term)));
                continue;
            }

            //If it occurs for the first time it can be skipped
            string current = variableTerm.GetValueOrThrow().Identifier;
            if (variablesInHead.Add(current))
            {
                continue;
            }

            var newVariable = new VariableTerm(
                ASPExtensions.GenerateUniqeName(current, _variables, "rwh"));

            //Rewrite the head
            terms[terms.IndexOf(term)] = newVariable;
            
            //Rewrite the body
            rule.Body.Literals.Insert(0,new NafLiteral(new BinaryOperation(
                variableTerm.GetValueOrThrow(), new Equality(), newVariable)));
        }
        
        return rule;
    }

    private Dictionary<(string,int), List<Statement>> PreprocessRules(List<Statement> rules)
    {
        //heads mapped to all bodies occuring in the program
        Dictionary<(string,int), List<Statement>> disjunctions = [];
        
        foreach (var rule in rules)
        {
            //Headless rules will be treated within nmr check
            if (!rule.HasHead) continue;

            var head = (rule.Head.Literal.Identifier, rule.Head.Literal.Terms.Count);
            
            if (!disjunctions.TryAdd(head, [rule]))
            {
                disjunctions[head].Add(rule);
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
                statements[0].Head.IsDual = true;

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
            var head = new Head(new ClassicalLiteral(
                disjunction.Key.Item1,
                false,
                [newVariable]));
            head.IsDual = true;
            newStatement.AddHead(head);
            
            //For readability add the statement beforehand
            duals.Add(newStatement);
            List<NafLiteral> newBody = [];

            for (var i = 0; i < statements.Count; i++)
            {
                var statement = statements[i];
                
                string tempVariableId =
                    ASPExtensions.GenerateUniqeName(statement.Head.Literal.Identifier, _ruleNames, "idis");
                
                statement.Head.Literal.Identifier = tempVariableId;
                
                //newBody.Add(new NafLiteral(new ClassicalLiteral(tempVariableId, false, [newVariable]), true));
                newBody.Add(new NafLiteral(new ClassicalLiteral(tempVariableId, false, [newVariable]), 
                    true));

                var withForall = AddForall(statement, _variables);
                if (withForall.Count > 0)
                {
                    duals.AddRange(withForall);
                    continue;
                }

                GetDualRules(ComputeHead(statement)).ForEach(
                    s =>
                    {
                        s.Head.Literal.Identifier = tempVariableId;
                        s.Head.IsDual = true;
                        duals.Add(s);
                    });
            }


            newStatement.AddBody(new Body(newBody));
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
            return GetDualRules([ComputeHead(stmt)]);
        }
        
        List<Statement> duals = [];

        var negator = new TypeNegator(new BinaryOperatorNegator(), new TermCopyVisitor());

        for (var i = 0; i < rule.Body.Literals.Count; i++)
        {
            var dualStatement = new Statement();
            
            //Maybe set negated on the classical literal in the head 
            //to indicate a dual rule
            rule.Head.IsDual = true;
            dualStatement.AddHead(rule.Head);

            List<NafLiteral> dualBody = [];
            //add preceding
            dualBody.AddRange(rule.Body.Literals[0..i]);
            
            //add negated
            var goal = rule.Body.Literals[i];
            var negated = negator.NegateNaf(goal);
            dualBody.Add(negated);
            
            dualStatement.AddBody(new Body(dualBody));
            
            duals.Add(dualStatement);
        }

        return duals;
    }

    private List<string> GetBodyVariables(Statement rule)
    {
        var head = rule.Head
            .Accept(_variableFinder)
            .GetValueOrThrow("Cannot retrieve variables from head!")
            .Select(v => v.Identifier);
        
        var body = rule.Body
            .Accept(_variableFinder)
            .GetValueOrThrow("Cannot retrieve variables from body!")
            .Select(v => v.Identifier);

        return body.Except(head).ToList();
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
        // 1) Compute Duals Normally but replace predicate in the head
        string newId = 
            ASPExtensions.GenerateUniqeName(rule.Head.Literal.Identifier, variablesInProgram, "fa");
        rule.Head.Literal.Identifier = newId;
        duals.AddRange(GetDualRules(rule));
        
        // 2) Body Variables added to head of each dual
        
        foreach (var variable in bodyVariables)
        {
            //They share the same head
            duals[0].Head.Literal.Terms.Add(new VariableTerm(variable));
        }
        
        // 3) Create Clause with forall over the new Predicate
        Statement forall = new();
        
        statement.Head.IsDual = true;
        forall.AddHead(statement.Head);
        NafLiteral innerGoal;
        //Made Disjunction
        if (duals.Count > 1)
        {
            //Inner goal has to be negated 
            innerGoal = new NafLiteral(new ClassicalLiteral(
                    newId, false, duals[0].Head.Literal.Terms),
                !duals[0].Head.Literal.Negated);
        }
        else
        {
            innerGoal = new NafLiteral(new ClassicalLiteral(
                    newId, false, duals[0].Head.Literal.Terms),
                duals[0].Head.Literal.Negated);
        }
        
        //append body with (nested) forall
        forall.AddBody(new Body([NestForall(bodyVariables.ToList(), innerGoal)]));
        
        duals.Insert(0,forall);
        
        return duals;
    }

    private static NafLiteral NestForall(List<string> bodyVariables, NafLiteral innerGoal)
    {
        if (bodyVariables.Count == 0)
        {
            return innerGoal;
        }
        
        string v = bodyVariables[0];
        bodyVariables.RemoveAt(0);

        NafLiteral result = NestForall(bodyVariables, innerGoal);
        
        //return new BasicTerm("forall", [ new VariableTerm(v), result]);
        var f = new Forall(new VariableTerm(v), result);
        string s = f.ToString();
        return f;
    }
}