using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors;

public class TermVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<Term>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<Term> VisitNegatedTerm(ASPParser.NegatedTermContext context)
    {
        var baseTerm = context.term().Accept(this);

        if (!baseTerm.HasValue)
        {
            _errorLogger.LogError("Cannot parse inner term!", context);
            return new None<Term>();
        }

        return new Some<Term>(new NegatedTerm(baseTerm.GetValueOrThrow()));
    }

    public override IOption<Term> VisitStringTerm(ASPParser.StringTermContext context)
    {
        var text = context.STRING().GetText();

        if (text == null)
        {
            _errorLogger.LogError("The string term must have a text!", context);
            return new None<Term>();
        }

        return new Some<Term>(new StringTerm(text[1..^1]));
    }

    public override IOption<Term> VisitBasicTerm(ASPParser.BasicTermContext context)
    {
        var id = context.ID().GetText();

        if (id == null)
        {
            _errorLogger.LogError("The term must have an identifier!", context);
            return new None<Term>();
        }

        var terms = context.terms();

        if (terms == null)
        {
            //Can be a basic term without any inner terms
            return new Some<Term>(new BasicTerm(id, []));
        }
        
        var innerTerms = terms.Accept(new TermsVisitor(errorLogger));

        if (innerTerms == null || !innerTerms.HasValue)
        {
            _errorLogger.LogError("Cannot parse inner terms!", context);
            return new None<Term>();
        }
        
        return new Some<Term>(new BasicTerm(id, innerTerms.GetValueOrThrow()));
    }

    public override IOption<Term> VisitArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext context)
    {
        var left = context.term(0).Accept(this);
        var right = context.term(1).Accept(this);
        var operation = context.arithop().Accept(new ArithmeticOperationVisitor(_errorLogger));
        
        if (!left.HasValue || !right.HasValue || !operation.HasValue)
        {
            _errorLogger.LogError("Cannot parse arithmetic operation!", context);
            return new None<Term>();
        }        
        
        return new Some<Term>(new ArithmeticOperationTerm(operation.GetValueOrThrow()));
    }

    public override IOption<Term> VisitParenthesizedTerm(ASPParser.ParenthesizedTermContext context)
    {
        var baseTerm = context.term().Accept(this);
        
        if (!baseTerm.HasValue)
        {
            _errorLogger.LogError("Cannot parse inner term!", context);
            return new None<Term>();
        }

        return new Some<Term>(baseTerm.GetValueOrThrow());
    }

    public override IOption<Term> VisitAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext context)
    {
        return new Some<Term>(new AnonymusVariableTerm());
    }

    public override IOption<Term> VisitNumberTerm(ASPParser.NumberTermContext context)
    {
        var textNumber = context.NUMBER().GetText();
        
        if (textNumber == null)
        {
            _errorLogger.LogError("Cannot find number!", context);
            return new None<Term>();
        }
        
        var isValid = int.TryParse(textNumber, out var number);

        if (!isValid)
        {
            _errorLogger.LogError("Term cannot be converted to number!", context);
            return new None<Term>();
        }
        
        return new Some<Term>(new NumberTerm(number));
    }

    public override IOption<Term> VisitVariableTerm(ASPParser.VariableTermContext context)
    {
        var variable = context.VARIABLE().GetText();

        if (variable == null)
        {
            _errorLogger.LogError("Cannot parse name of the variable!", context);
            return new None<Term>();
        }
        
        return new Some<Term>(new VariableTerm(variable));
    }
}