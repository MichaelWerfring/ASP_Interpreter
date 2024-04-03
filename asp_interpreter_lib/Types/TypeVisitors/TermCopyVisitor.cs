using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class TermCopyVisitor : TypeBaseVisitor<ITerm>
{
    public override IOption<ITerm> Visit(AnonymusVariableTerm term)
    {
        return new Some<ITerm>(new AnonymusVariableTerm());
    }

    public override IOption<ITerm> Visit(VariableTerm term)
    {
        return new Some<ITerm>(new VariableTerm(term.ToString()));
    }

    public override IOption<ITerm> Visit(ArithmeticOperationTerm term)
    {
        return new Some<ITerm>(new ArithmeticOperationTerm(term.Operation));
    }

    public override IOption<ITerm> Visit(BasicTerm term)
    {
        List<ITerm> children = [];

        term.Terms.ForEach(t =>
        {
            children.Add(t.Accept(this).GetValueOrThrow("" +
                "The child term cannot be parsed!")); 
        });

        return new Some<ITerm>(new BasicTerm(term.Identifier.ToString(), children));
    }

    public override IOption<ITerm> Visit(StringTerm term)
    {
        return new Some<ITerm>(new StringTerm(term.Value.ToString()));
    }

    public override IOption<ITerm> Visit(NumberTerm term)
    {
        return new Some<ITerm>(new NumberTerm(term.Value));
    }

    public override IOption<ITerm> Visit(NegatedTerm term)
    {
        var innerTerm = term.Accept(this);
        return new Some<ITerm>(new NegatedTerm(
            innerTerm.GetValueOrThrow("The inner term cannot be copied!")));
    }

    public override IOption<ITerm> Visit(ParenthesizedTerm term)
    {
        var innerTerm = term.Accept(this);
        return new Some<ITerm>(new ParenthesizedTerm(
            innerTerm.GetValueOrThrow("The inner term cannot be copied!")));
    }
}