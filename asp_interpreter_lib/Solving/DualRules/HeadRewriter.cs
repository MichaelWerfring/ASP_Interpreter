﻿using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving.DualRules;

public class HeadRewriter : TypeBaseVisitor<Statement>
{
    private readonly PrefixOptions _options;
    
    private readonly Statement _statement;
    
    private readonly Literal _head;
    
    private readonly HashSet<string> _variables;

    private readonly TermCopyVisitor _termCopyVisitor = new();

    private int _counter;
    
    public HeadRewriter(PrefixOptions options, Statement statement)
    {
        _options = options;
        _statement = statement;
        _head = statement.Head.GetValueOrThrow("Cannot rewrite headless statement!");
        _variables = new HashSet<string>();
        //var variableGetter = new VariableFinder();
        //var terms = statement.Accept(variableGetter).GetValueOrThrow("Cannot retrieve variables from program!");
        //terms.ForEach(t => _variables.Add(t.Identifier));
        _counter = 0;
    }


    public override IOption<Statement> Visit(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        
        statement.Head.IfHasValue(h => h.Accept(this));

        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(Literal classicalLiteral)
    {
        ArgumentNullException.ThrowIfNull(classicalLiteral);

        for (var index = 0; index < classicalLiteral.Terms.Count; index++)
        {
            var term = classicalLiteral.Terms[index];
            term.Accept(this);
        }

        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        if (_variables.Add(term.Identifier))
        {
            return new Some<Statement>(_statement);    
        }
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
        
        var oldTerm = term.Accept(_termCopyVisitor).GetValueOrThrow("Cannot copy term!");
        
        
        //Rewrite the head
        term.Identifier = newVariable.Identifier;
            
        //Rewrite the body
        _statement.Body.Insert(0,new BinaryOperation(
            term, new Equality(), oldTerm));
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        foreach (var child in term.Terms)
        {
            child.Accept(this);
        }

        if (term.Terms.Count == 0)
        {
            var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
            int i = _head.Terms.IndexOf(term);
            if (i == -1)
            {
                throw new InvalidOperationException("The given term is not contained in the head!");
            }
         
            _head.Terms[i] = newVariable;
            _statement.Body.Insert(0,new BinaryOperation(
                newVariable, new Equality(), term));
        }
        
        return new Some<Statement>(_statement);
    }
    
    public override IOption<Statement> Visit(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);

        //replace head
        //_statement.Head.Literal?.Terms[_statement.Head.Literal?.Terms.IndexOf(term)] = newHeadVariable;
        int i = _head.Terms.IndexOf(term);
        
        if (i == -1)
        {
            throw new InvalidOperationException("The given term is not contained in the head!");
        }
         
        _head.Terms[i] = newVariable;
                
        //replace body
        _statement.Body.Insert(0,new BinaryOperation(
            newVariable, new Equality(), term));
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);

        //replace head
        //_statement.Head.Literal?.Terms[_statement.Head.Literal?.Terms.IndexOf(term)] = newHeadVariable;
        int i = _head.Terms.IndexOf(term);
        
        if (i == -1)
        {
            throw new InvalidOperationException("The given term is not contained in the head!");
        }
         
        _head.Terms[i] = newVariable;
                
        //replace body
        _statement.Body.Insert(0,new BinaryOperation(
            newVariable, new Equality(), term));
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        term.Term.Accept(this);
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        term.Term.Accept(this);
        return new Some<Statement>(_statement);
    }
}