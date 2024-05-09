using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class TermVisitor(ILogger logger) : ASPBaseVisitor<IOption<ITerm>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<ITerm> VisitNegatedTerm(ASPParser.NegatedTermContext context)
    {
        var baseTerm = context.term().Accept(this);

        if (!baseTerm.HasValue)
        {
            _logger.LogError("Cannot parse inner term!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new NegatedTerm(baseTerm.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitStringTerm(ASPParser.StringTermContext context)
    {
        var text = context.STRING().GetText();

        if (text == null)
        {
            _logger.LogError("The string term must have a text!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new StringTerm(text[1..^1]));
    }

    public override IOption<ITerm> VisitBasicTerm(ASPParser.BasicTermContext context)
    {
        var id = context.ID().GetText();

        if (id == null)
        {
            _logger.LogError("The term must have an identifier!", context);
            return new None<ITerm>();
        }

        var terms = context.terms();

        if (terms == null)
        {
            //Can be a basic term without any inner terms
            return new Some<ITerm>(new BasicTerm(id, []));
        }
        
        var innerTerms = terms.Accept(new TermsVisitor(_logger));

        if (innerTerms == null || !innerTerms.HasValue)
        {
            _logger.LogError("Cannot parse inner terms!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new BasicTerm(id, innerTerms.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext context)
    {
        var left = context.term(0).Accept(this);
        var right = context.term(1).Accept(this);
        var operation = context.arithop().Accept(new ArithmeticOperationVisitor(_logger));
        
        if (!left.HasValue || !right.HasValue || !operation.HasValue)
        {
            _logger.LogError("Cannot parse arithmetic operation!", context);
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
            _logger.LogError("Cannot parse inner term!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(baseTerm.GetValueOrThrow());
    }

    public override IOption<ITerm> VisitAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext context)
    {
        return new Some<ITerm>(new AnonymousVariableTerm());
    }

    public override IOption<ITerm> VisitNumberTerm(ASPParser.NumberTermContext context)
    {
        var textNumber = context.NUMBER().GetText();
        
        if (textNumber == null)
        {
            _logger.LogError("Cannot find number!", context);
            return new None<ITerm>();
        }
        
        var isValid = int.TryParse(textNumber, out var number);

        if (!isValid)
        {
            _logger.LogError("Term cannot be converted to number!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new NumberTerm(number));
    }

    public override IOption<ITerm> VisitVariableTerm(ASPParser.VariableTermContext context)
    {
        var variable = context.VARIABLE().GetText();

        if (variable == null)
        {
            _logger.LogError("Cannot parse name of the variable!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new VariableTerm(variable));
    }

    public override IOption<ITerm> VisitListTerm(ASPParser.ListTermContext context)
    {
        var list =context.list().Accept(this);

        if (list == null|| !list.HasValue)
        {
            _logger.LogError("Cannot parse list!", context);
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
        
        var terms = innerList.Accept(new TermsVisitor(_logger));
        
        if(!terms.HasValue)
        {
            _logger.LogError("Cannot parse list terms!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new ConventionalList(terms.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitRecursiveList(ASPParser.RecursiveListContext context)
    {
        var head = context.term(0).Accept(new TermVisitor(_logger));
        var tail = context.term(1).Accept(new TermVisitor(_logger));

        if (!head.HasValue)
        {
            _logger.LogError("Cannot parse head term!", context);
            return new None<ITerm>();
        }
        
        if (!tail.HasValue)
        {
            _logger.LogError("Cannot parse tail term!", context);
            return new None<ITerm>();
        }
        
        return new Some<ITerm>(new RecursiveList(head.GetValueOrThrow(), tail.GetValueOrThrow()));
    }
}