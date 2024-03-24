using asp_interpreter_lib.Types.Terms.TermConversion;

namespace asp_interpreter_lib.Types.Terms;

public class TermToNumberConverter : ITermVisitor<int>
{
    public int Visit(BasicTerm term)
    {
        throw new InvalidOperationException("Cannot convert basic term to a number!"); 
    }

    public int Visit(AnonymusVariableTerm term)
    {
        throw new InvalidOperationException("Cannot convert _ to a number!"); 
    }

    public int Visit(VariableTerm term)
    {
        throw new NotImplementedException();
    }

    public int Visit(ArithmeticOperationTerm term)
    {
        var result = term.Operation.Evaluate();
        return result.Accept(this);
    }

    public int Visit(ParenthesizedTerm term)
    {
        return term.Accept(this);
    }

    public int Visit(StringTerm term)
    {
        throw new InvalidOperationException("Cannot convert a string to a number!"); 
    }

    public int Visit(NegatedTerm term)
    {
        return -term.Term.Accept(this);
    }

    public int Visit(NumberTerm term)
    {
        return term.Value;
    }
}