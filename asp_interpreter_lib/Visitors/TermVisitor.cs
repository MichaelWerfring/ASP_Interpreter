﻿using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class TermVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<ITerm>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<ITerm> VisitNegatedTerm(ASPParser.NegatedTermContext context)
    {
        var baseTerm = context.term().Accept(this);

        if (!baseTerm.HasValue)
        {
            _errorLogger.LogError("Cannot parse inner term!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new NegatedTerm(baseTerm.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitStringTerm(ASPParser.StringTermContext context)
    {
        var text = context.STRING().GetText();

        if (text == null)
        {
            _errorLogger.LogError("The string term must have a text!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new StringTerm(text[1..^1]));
    }

    public override IOption<ITerm> VisitBasicTerm(ASPParser.BasicTermContext context)
    {
        var id = context.ID().GetText();

        if (id == null)
        {
            _errorLogger.LogError("The term must have an identifier!", context);
            return new None<ITerm>();
        }

        var terms = context.terms();

        if (terms == null)
        {
            //Can be a basic term without any inner terms
            return new Some<ITerm>(new BasicTerm(id, []));
        }
        
        var innerTerms = terms.Accept(new TermsVisitor(errorLogger));

        if (innerTerms == null || !innerTerms.HasValue)
        {
            _errorLogger.LogError("Cannot parse inner terms!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new BasicTerm(id, innerTerms.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext context)
    {
        var left = context.term(0).Accept(this);
        var right = context.term(1).Accept(this);
        var operation = context.arithop().Accept(new ArithmeticOperationVisitor(_errorLogger));
        
        if (!left.HasValue || !right.HasValue || !operation.HasValue)
        {
            _errorLogger.LogError("Cannot parse arithmetic operation!", context);
            return new None<ITerm>();
        }        
        
        return new Some<ITerm>(new ArithmeticOperationTerm(
            left.GetValueOrThrow(),
            operation.GetValueOrThrow(),
            right.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitParenthesizedTerm(ASPParser.ParenthesizedTermContext context)
    {
        var baseTerm = context.term().Accept(this);
        
        if (!baseTerm.HasValue)
        {
            _errorLogger.LogError("Cannot parse inner term!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(baseTerm.GetValueOrThrow());
    }

    public override IOption<ITerm> VisitAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext context)
    {
        return new Some<ITerm>(new AnonymusVariableTerm());
    }

    public override IOption<ITerm> VisitNumberTerm(ASPParser.NumberTermContext context)
    {
        var textNumber = context.NUMBER().GetText();
        
        if (textNumber == null)
        {
            _errorLogger.LogError("Cannot find number!", context);
            return new None<ITerm>();
        }
        
        var isValid = int.TryParse(textNumber, out var number);

        if (!isValid)
        {
            _errorLogger.LogError("Term cannot be converted to number!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new NumberTerm(number));
    }

    public override IOption<ITerm> VisitVariableTerm(ASPParser.VariableTermContext context)
    {
        var variable = context.VARIABLE().GetText();

        if (variable == null)
        {
            _errorLogger.LogError("Cannot parse name of the variable!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new VariableTerm(variable));
    }

    public override IOption<ITerm> VisitListTerm(ASPParser.ListTermContext context)
    {
        var list =context.list().Accept(this);

        if (list == null|| !list.HasValue)
        {
            _errorLogger.LogError("Cannot parse list!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(list.GetValueOrThrow());
    }
    
    public override IOption<ITerm> VisitConventionalList(ASPParser.ConventionalListContext context)
    {
        var innerList = context.terms();

        if (innerList == null)
        {
            return new Some<ITerm>(new ConventionalList([]));
        }
        
        var terms = innerList.Accept(new TermsVisitor(_errorLogger));
        
        if(!terms.HasValue)
        {
            _errorLogger.LogError("Cannot parse list terms!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new ConventionalList(terms.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitRecursiveList(ASPParser.RecursiveListContext context)
    {
        var head = context.term(0).Accept(new TermVisitor(_errorLogger));
        var tail = context.term(1).Accept(new TermVisitor(_errorLogger));

        if (!head.HasValue)
        {
            _errorLogger.LogError("Cannot parse head term!", context);
            return new None<ITerm>();
        }
        
        if (!tail.HasValue)
        {
            _errorLogger.LogError("Cannot parse tail term!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new RecursiveList(head.GetValueOrThrow(), tail.GetValueOrThrow()));
    }
}