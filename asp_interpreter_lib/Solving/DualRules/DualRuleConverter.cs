﻿using asp_interpreter_lib.Types;
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
    
    public DualRuleConverter(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);
        _variables = new HashSet<string>();
        _variableTermConverter = new VariableTermConverter();
        _ruleNames = program.Accept(new RuleNameFinder()).
            GetValueOrThrow("Cannot retrieve rule names from program!");
        
        
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
            if (_variables.Add(current))
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

                foreach (var statement in GetDualRules(statements[0]))
                {
                    duals.Add(statement);
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
                
                string tempVariableId = ASPExtensions.GenerateUniqeName(statement.Head.Literal.Identifier, _ruleNames, "idis");
                //tempStatement.AddHead(new Head(new ClassicalLiteral(tempVariableId, statement.Head.Literal.Negated, statement.Head.Literal.Terms)));
                statement.Head.Literal.Identifier = tempVariableId;
                
                newBody.Add(new NafLiteral(new ClassicalLiteral(tempVariableId, false, [newVariable]), true));

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

    private static List<string> GetBodyVariables(Statement rule)
    {
        var result = new Statement();
        
        var variableGetter = new GetVariableVisitor();
        var headVariables = rule.Head.Accept(variableGetter)
            .GetValueOrThrow("Cannot retrieve variables from head!");
        var bodyVariables = rule.Body.Accept(variableGetter)
            .GetValueOrThrow("Cannot retrieve variables from body!");

        return bodyVariables.Except(headVariables).ToList();
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
        
        var copier = new StatementCopyVisitor();
        Statement rule = statement.Accept(copier).
            GetValueOrThrow("Cannot retrieve copy of statement");
        
        List<Statement> duals = [];
        // 1) Compute Duals Normally but replace predicate in the head
        string oldId = rule.Head.Literal.Identifier; 
        
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
        if (statement.HasHead)
        {
            statement.Head.IsDual = true;
            forall.AddHead(statement.Head);    
        }

        //BasicTerm innerTerm = new BasicTerm(
            //newId, rule.Body.Literals[0].ClassicalLiteral.Terms);
            BasicTerm innerTerm = new BasicTerm(newId, duals[0].Head.Literal.Terms);
            
            
        string firstBodyVariable = bodyVariables[0];
        bodyVariables.RemoveAt(0);
        BasicTerm forallTerm = NestForall(bodyVariables.ToList(), innerTerm);
        
        Body forallBody = new Body([
            new NafLiteral(new ClassicalLiteral(
                "forall", false, [
                    new VariableTerm(firstBodyVariable),
                    forallTerm]
            ), false)
        ]);
        forall.AddBody(forallBody);
        
        duals.Insert(0,forall);
        
        return duals;
    }

    private static BasicTerm NestForall(List<string> bodyVariables, BasicTerm innerTerm)
    {
        if (bodyVariables.Count == 0)
        {
            return innerTerm;
        }
        
        string v = bodyVariables[0];
        bodyVariables.RemoveAt(0);

        BasicTerm result = NestForall(bodyVariables, innerTerm);
        
        return new BasicTerm("forall", [ new VariableTerm(v), result]);
    }
}