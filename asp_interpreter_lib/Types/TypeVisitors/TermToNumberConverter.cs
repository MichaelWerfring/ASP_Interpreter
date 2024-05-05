using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class TermToNumberConverter : TypeBaseVisitor<int>
{
    public override IOption<int> Visit(BasicTerm term)
    {
        return new None<int>();
    }

    public override IOption<int> Visit(AnonymusVariableTerm term)
    {
        return new None<int>(); 
    }

    public override IOption<int> Visit(VariableTerm term)
    {
        return new None<int>();
    }

    public override IOption<int> Visit(ArithmeticOperationTerm term)
    {
        var left = term.Left.Accept(this);
        var right = term.Left.Accept(this);

        if (!left.HasValue || !right.HasValue)
        {
            return new None<int>();
        }

        return new Some<int>(term.Operation.Evaluate(left.GetValueOrThrow(), right.GetValueOrThrow()));
    }

    public override IOption<int> Visit(ParenthesizedTerm term)
    {
        var result = term.Term.Accept(this);
        
        if (result.HasValue)
        {
            return new Some<int>(result.GetValueOrThrow());
        }
        
        return new None<int>();
    }

    public override IOption<int> Visit(StringTerm term)
    {
        return new None<int>();
    }

    public override IOption<int> Visit(NegatedTerm term)
    {
        var result = term.Term.Accept(this); 
        
        if (result.HasValue)
        {
            return new Some<int>(-result.GetValueOrThrow());
        }
        
        return new None<int>();
    }

    public override IOption<int> Visit(NumberTerm term)
    {
        return new Some<int>(term.Value);
    }
}