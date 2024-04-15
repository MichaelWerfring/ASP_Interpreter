using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class TermConverter : TypeBaseVisitor<ISimpleTerm>
{
    private StringTermConverter _stringConverter = new StringTermConverter();

    public ISimpleTerm Convert(ITerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var result = term.Accept(this);
        ISimpleTerm convertedTerm;
        try
        {
            convertedTerm = result.GetValueOrThrow();
        }
        catch
        {
            throw new InvalidDataException($"{nameof(term)} contained unconvertable types.");
        }

        return convertedTerm;
    }

    public override IOption<ISimpleTerm> Visit(AnonymusVariableTerm _)
    {
        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(new Variable(term.Identifier));
    }

    public override IOption<ISimpleTerm> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        ArithmeticOperationConverter converter = new ArithmeticOperationConverter(term, this);

        return new Some<ISimpleTerm>(converter.Convert());
    }

    public override IOption<ISimpleTerm> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var newChildren = new ISimpleTerm[term.Terms.Count];
        for (int i = 0; i < term.Terms.Count; i++)
        {
            var result = term.Terms[i].Accept(this);
            try
            {
                newChildren[i] = result.GetValueOrThrow();
            }
            catch
            {
                return new None<ISimpleTerm>();
            }
        }
        return new Some<ISimpleTerm>(new Structure(term.Identifier, newChildren));
    }

    public override IOption<ISimpleTerm> Visit(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(_stringConverter.Convert(term));
    }

    public override IOption<ISimpleTerm> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(new Integer(term.Value));
    }

    public override IOption<ISimpleTerm> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var innerTermResult = term.Term.Accept(this);
        ISimpleTerm innerTerm;
        try
        {
            innerTerm = innerTermResult.GetValueOrThrow();
        }
        catch
        {
            return new None<ISimpleTerm>();
        }

        return new Some<ISimpleTerm>(new ClassicalNegation(innerTerm));
    }

    public override IOption<ISimpleTerm> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var inner = term.Term.Accept(this);
        try
        {
            return new Some<ISimpleTerm>(new Parenthesis(inner.GetValueOrThrow()));
        }
        catch
        {
            return new None<ISimpleTerm>();
        }
    }
}
