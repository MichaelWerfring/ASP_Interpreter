using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;

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
        throw new NotImplementedException();
    }

    public override IOption<int> Visit(ArithmeticOperationTerm term)
    {
        var result = term.Operation.Evaluate();
        var number = result.Accept(this);

        if (number.HasValue)
        {
            return new Some<int>(number.GetValueOrThrow());
        }
        
        return new None<int>();
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