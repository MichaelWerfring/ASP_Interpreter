using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

public class NegatedTermConverter : TypeBaseVisitor<ISimpleTerm>
{
    private readonly TermConverter _converter;
    private readonly FunctorTableRecord _record;

    public NegatedTermConverter(TermConverter converter, FunctorTableRecord record)
    {
        ArgumentNullException.ThrowIfNull(converter, nameof(converter));
        ArgumentNullException.ThrowIfNull(record, nameof(record));

        _converter = converter;
        _record = record;
    }

    public ISimpleTerm Convert(NegatedTerm negatedTerm)
    {
        ArgumentNullException.ThrowIfNull(negatedTerm, nameof(negatedTerm));

        var result = negatedTerm.Term.Accept(this);
        if (!result.HasValue)
        {
            throw new ArgumentException("Inner term was unconvertable!");
        }

        return result.GetValueOrThrow();
    }

    public override IOption<ISimpleTerm> Visit(BasicTerm term)
    {
        var convertedStructure = _converter.Convert(term);

        return new Some<ISimpleTerm>(new Structure(_record.ClassicalNegation, [convertedStructure]));
    }

    public override IOption<ISimpleTerm> Visit(NegatedTerm term)
    {
        var convertedTerm = _converter.Convert(term.Term);

        return new Some<ISimpleTerm>(convertedTerm);
    }

    public override IOption<ISimpleTerm> Visit(NumberTerm term)
    {
        var negatedInteger = new Integer(term.Value * -1);

        return new Some<ISimpleTerm>(negatedInteger);
    }

    // unconvertible terms : these should not be inside a negated term
    public override IOption<ISimpleTerm> Visit(AnonymusVariableTerm _)
    {
        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(ArithmeticOperationTerm term)
    {
        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(ConventionalList term)
    {
        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(ParenthesizedTerm term)
    {
        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(RecursiveList term)
    {
        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(StringTerm term)
    {
        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(VariableTerm term)
    {
        return new None<ISimpleTerm>();
    }
}
