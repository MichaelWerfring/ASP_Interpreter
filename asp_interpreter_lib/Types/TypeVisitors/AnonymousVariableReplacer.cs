﻿using System.Runtime.CompilerServices;
using Asp_interpreter_lib.Types.ArithmeticOperations;
using Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Preprocessing;

namespace Asp_interpreter_lib.Types.TypeVisitors;

public class AnonymousVariableReplacer : TypeBaseVisitor<ITerm>
{
    private readonly PrefixOptions _prefixOptions;

    private int _replacementCount;
    
    public AnonymousVariableReplacer(PrefixOptions prefixOptions)
    {
        ArgumentNullException.ThrowIfNull(prefixOptions);
        _prefixOptions = prefixOptions;
        _replacementCount = 0;
    }
    
    public Statement Replace(Statement statement)
    {
        _replacementCount = 0;
        statement.Accept(this);
        _replacementCount = 0;
        return statement;
    }
    
    public override IOption<ITerm> Visit(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        
        if (statement.HasHead)
        {
            statement.Head.GetValueOrThrow().Accept(this);
        }

        if (statement.HasBody)
        {
            foreach (var goal in statement.Body)
            {
                goal.Accept(this);
            }
        }
        
        return new None<ITerm>();
    }

    public override IOption<ITerm> Visit(Literal literal)
    {
        ArgumentNullException.ThrowIfNull(literal);
        List<ITerm> terms = [];
        for (var i = 0; i < literal.Terms.Count; i++)
        {
            var term = literal.Terms[i];
            literal.Terms[i] = term.Accept(this).GetValueOrThrow(); 
        }

        return new None<ITerm>();
    }

    public override IOption<ITerm> Visit(BinaryOperation binOp)
    {
        ArgumentNullException.ThrowIfNull(binOp);
        
        binOp.Left = binOp.Left.Accept(this).GetValueOrThrow();
        binOp.Right = binOp.Right.Accept(this).GetValueOrThrow();

        return new None<ITerm>();
    }

    public override IOption<ITerm> Visit(AnonymousVariableTerm variable)
    {
        ArgumentNullException.ThrowIfNull(variable);
        string newName = _prefixOptions.VariablePrefix + _replacementCount++ + "_";
        return new Some<ITerm>(new VariableTerm(newName));
    }

    public override IOption<ITerm> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        return new Some<ITerm>(term);
    }

    public override IOption<ITerm> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var left = term.Left.Accept(this).GetValueOrThrow();
        var right = term.Right.Accept(this).GetValueOrThrow();
        
        
        return new Some<ITerm>(new ArithmeticOperationTerm(left, term.Operation, right));
    }

    public override IOption<ITerm> Visit(BasicTerm basicTerm)
    {
        ArgumentNullException.ThrowIfNull(basicTerm);
        List<ITerm> terms = [];

        for (var i = 0; i < basicTerm.Terms.Count; i++)
        {
            var term = basicTerm.Terms[i];
            basicTerm.Terms[i] = term.Accept(this).
                GetValueOrThrow();
        }

        return new Some<ITerm>(basicTerm);
    }
    
    public override IOption<ITerm> Visit(StringTerm term)
    {
        return new Some<ITerm>(term);
    }

    public override IOption<ITerm> Visit(NumberTerm term)
    {
        return new Some<ITerm>(term);
    }

    public override IOption<ITerm> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var newTerm = term.Term.Accept(this).GetValueOrThrow();

        return new Some<ITerm>(new NegatedTerm(newTerm));
    }

    public override IOption<ITerm> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        var newTerm = term.Term.Accept(this).GetValueOrThrow();

        return new Some<ITerm>(new ParenthesizedTerm(newTerm));
    }

    public override IOption<ITerm> Visit(RecursiveList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        List<ITerm> terms = [];

        var head = list.Head.Accept(this).GetValueOrThrow();
        var tail = list.Tail.Accept(this).GetValueOrThrow();

        return new Some<ITerm>(new RecursiveList(head, tail));
    }

    public override IOption<ITerm> Visit(ConventionalList list)
    {
        ArgumentNullException.ThrowIfNull(list);

        for (var i = 0; i < list.Terms.Count; i++)
        {
            var term = list.Terms[i];
            list.Terms[i] = term.Accept(this).GetValueOrThrow();
        }

        return new Some<ITerm>(list);
    }
}