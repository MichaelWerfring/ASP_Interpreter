using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Visitors;

public class TermVisitor : ASPParserBaseVisitor<IOption<ITerm>>
{
    private readonly ILogger logger;

    public TermVisitor(ILogger logger)
    {
        this.logger = logger ??
            throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
    }

    public override IOption<ITerm> VisitNegatedTerm(ASPParser.NegatedTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var baseTerm = context.term().Accept(this);

        if (!baseTerm.HasValue)
        {
            this.logger.LogError("Cannot parse inner term!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new NegatedTerm(baseTerm.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitStringTerm(ASPParser.StringTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var text = context.STRING().GetText();

        if (text == null)
        {
            logger.LogError("The string term must have a text!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new StringTerm(text[1..^1]));
    }

    public override IOption<ITerm> VisitBasicTerm(ASPParser.BasicTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var id = context.ID().GetText();

        if (id == null)
        {
            this.logger.LogError("The term must have an identifier!", context);
            return new None<ITerm>();
        }

        var terms = context.terms();

        if (terms == null)
        {
            // Can be a basic term without any inner terms
            return new Some<ITerm>(new BasicTerm(id, []));
        }

        var innerTerms = terms.Accept(new TermsVisitor(this.logger));

        if (innerTerms == null || !innerTerms.HasValue)
        {
            this.logger.LogError("Cannot parse inner terms!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new BasicTerm(id, innerTerms.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var left = context.term(0).Accept(this);
        var right = context.term(1).Accept(this);
        var operation = context.arithop().Accept(new ArithmeticOperationVisitor());

        if (!left.HasValue || !right.HasValue || !operation.HasValue)
        {
            this.logger.LogError("Cannot parse arithmetic operation!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new ArithmeticOperationTerm(
            left.GetValueOrThrow(),
            operation.GetValueOrThrow(),
            right.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitParenthesizedTerm(ASPParser.ParenthesizedTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var baseTerm = context.term().Accept(this);

        if (!baseTerm.HasValue)
        {
            this.logger.LogError("Cannot parse inner term!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(baseTerm.GetValueOrThrow());
    }

    public override IOption<ITerm> VisitAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return new Some<ITerm>(new AnonymousVariableTerm());
    }

    public override IOption<ITerm> VisitNumberTerm(ASPParser.NumberTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var textNumber = context.NUMBER().GetText();

        if (textNumber == null)
        {
            this.logger.LogError("Cannot find number!", context);
            return new None<ITerm>();
        }

        var isValidNumber = int.TryParse(textNumber, out var number);

        if (!isValidNumber)
        {
            logger.LogError("Term cannot be converted to number!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new NumberTerm(number));
    }

    public override IOption<ITerm> VisitVariableTerm(ASPParser.VariableTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var variable = context.VARIABLE().GetText();

        if (variable == null)
        {
            this.logger.LogError("Cannot parse name of the variable!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new VariableTerm(variable));
    }

    public override IOption<ITerm> VisitListTerm(ASPParser.ListTermContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var list = context.list().Accept(this);

        if (list == null || !list.HasValue)
        {
            this.logger.LogError("Cannot parse list!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(list.GetValueOrThrow());
    }

    public override IOption<ITerm> VisitConventionalList(ASPParser.ConventionalListContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var innerList = context.terms();

        if (innerList == null)
        {
            return new Some<ITerm>(new ConventionalList([]));
        }

        var terms = innerList.Accept(new TermsVisitor(this.logger));

        if (!terms.HasValue)
        {
            this.logger.LogError("Cannot parse list terms!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new ConventionalList(terms.GetValueOrThrow()));
    }

    public override IOption<ITerm> VisitRecursiveList(ASPParser.RecursiveListContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var head = context.term(0).Accept(this);
        var tail = context.term(1).Accept(this);

        if (!head.HasValue)
        {
            this.logger.LogError("Cannot parse head term!", context);
            return new None<ITerm>();
        }

        if (!tail.HasValue)
        {
            this.logger.LogError("Cannot parse tail term!", context);
            return new None<ITerm>();
        }

        return new Some<ITerm>(new RecursiveList(head.GetValueOrThrow(), tail.GetValueOrThrow()));
    }
}