using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public abstract class BinaryOperator
{
    public abstract bool Evaluate(Term left, Term right);
}