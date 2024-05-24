using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.FunctorNaming;

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

    public IOption<ISimpleTerm> Convert(NegatedTerm negatedTerm)
    {
        ArgumentNullException.ThrowIfNull(negatedTerm);

        return negatedTerm.Term.Accept(this);
    }

    public override IOption<ISimpleTerm> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var convertedStructure = _converter.Convert(term);

        return new Some<ISimpleTerm>(new Structure(_record.ClassicalNegation, [convertedStructure]));
    }

    public override IOption<ISimpleTerm> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var convertedTerm = _converter.Convert(term.Term);

        return new Some<ISimpleTerm>(convertedTerm);
    }

    public override IOption<ISimpleTerm> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var negatedInteger = new Integer(term.Value * -1);

        return new Some<ISimpleTerm>(negatedInteger);
    }

    // unconvertible terms : these should not be inside a negated term
    public override IOption<ISimpleTerm> Visit(AnonymousVariableTerm _)
    {
        ArgumentNullException.ThrowIfNull(_);

        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(ConventionalList term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(RecursiveList term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }
}
