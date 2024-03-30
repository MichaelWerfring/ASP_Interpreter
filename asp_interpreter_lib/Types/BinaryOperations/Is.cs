using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class Is : BinaryOperator
{
    public override bool Evaluate(Term left, Term right)
    {
        //Left term can only be a variable 
        //Right term must be arithmetic operation/evaluate to a number in the end
        throw new NotImplementedException();
    }
    
    public override string ToString()
    {
        return "is";
    }
}