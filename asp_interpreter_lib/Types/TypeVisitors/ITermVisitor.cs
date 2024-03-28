using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.TypeVisitors;

//If a Visitor with return value is needed
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

//If a Visitor without return value is needed
public interface ITermVisitor
{
    void Visit(BasicTerm term);
    
    void Visit(AnonymusVariableTerm term);
    
    void Visit(VariableTerm term);
    
    void Visit(ArithmeticOperationTerm term);
    
    void Visit(ParenthesizedTerm term);
    
    void Visit(StringTerm term);
    
    void Visit(NegatedTerm term);
    
    void Visit(NumberTerm term);
}