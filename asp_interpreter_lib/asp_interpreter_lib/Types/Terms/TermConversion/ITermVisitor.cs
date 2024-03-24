namespace asp_interpreter_lib.Types.Terms.TermConversion;

public interface ITermVisitor<T>
{
    T Visit(BasicTerm term);
    
    T Visit(AnonymusVariableTerm term);
    
    T Visit(VariableTerm term);
    
    T Visit(ArithmeticOperationTerm term);
    
    T Visit(ParenthesizedTerm term);
    
    T Visit(StringTerm term);
    
    T Visit(NegatedTerm term);
    
    T Visit(NumberTerm term);
}