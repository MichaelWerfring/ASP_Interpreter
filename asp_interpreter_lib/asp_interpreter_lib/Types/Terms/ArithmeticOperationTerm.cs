using asp_interpreter_lib.Types.ArithmeticOperations;

namespace asp_interpreter_lib.Types.Terms;

public class ArithmeticOperationTerm : Term
{
    public ArithmeticOperationTerm(Term left, ArithmeticOperation operation, Term right)
    {
        Left = left;
        Operation = operation;
        Right = right;
    }

    public ArithmeticOperation Operation { get; set; }
    
    public Term Left { get; set; }
    
    public Term Right { get; set; }
}