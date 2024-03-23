using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors.TermVisitors;

public class TermVisitor : ASPBaseVisitor<Term>
{
    public override Term VisitNegatedTerm(ASPParser.NegatedTermContext context)
    {
        return new NegatedTerm(context.term().Accept(this));
    }

    public override Term VisitStringTerm(ASPParser.StringTermContext context)
    {
        var text = context.STRING().GetText() ?? string.Empty;
        // Remove the quotes from the string
        return new StringTerm(text[1..^1]);
    }

    public override Term VisitBasicTerm(ASPParser.BasicTermContext context)
    {
        var id = context.ID().GetText() 
                 ?? throw new ArgumentException($"The given term has no id!");
        
        List<Term> terms = [];

        var childTerms = context.terms();

        if (childTerms == null)
        {
            return new BasicTerm(id, terms);
        }
        
        foreach (var term in childTerms.children)
        {
            terms.Add(term.Accept(this));
        }

        return new BasicTerm(id, terms);
    }

    public override Term VisitArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext context)
    {
        var left = context.term(0).Accept(this) 
                   ?? throw new ArgumentException("Cannot find left side of arithmetic operation!");
        
        var right = context.term(1).Accept(this) 
                    ?? throw new ArgumentException("Cannot find right side of arithmetic operation!");

        var operation = context.arithop().Accept(new ArithmeticOperationVisitor()) 
                        ?? throw new ArgumentException("The given arithmetic operation is not valid!");
        
        return new ArithmeticOperationTerm(left, operation, right);
    }

    public override Term VisitParenthesizedTerm(ASPParser.ParenthesizedTermContext context)
    {
        return context.term().Accept(this);
    }

    public override Term VisitAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext context)
    {
        return new AnonymusVariableTerm();
    }

    public override Term VisitNumberTerm(ASPParser.NumberTermContext context)
    {
        var isValid = int.TryParse(context.NUMBER().GetText(), out var number);

        if (!isValid)
        {
            throw new ArgumentException("The given number is not valid!");
        }
        
        return new NumberTerm(number);
    }

    public override Term VisitVariableTerm(ASPParser.VariableTermContext context)
    {
        var variable = context.VARIABLE().GetText() 
                       ?? throw new ArgumentException("Cannot find variable name!");
        return new VariableTerm(variable);
    }
}