using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Mapping;

public class TermConverter : TypeBaseVisitor<ISimpleTerm>
{
    private FunctorTableRecord _functorTable;

    private OperatorConverter _operatorConverter;

    public TermConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable, nameof(functorTable));

        _functorTable = functorTable;
        _operatorConverter = new OperatorConverter(functorTable);
    }

    public IOption<ISimpleTerm> Convert(ITerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public override IOption<ISimpleTerm> Visit(AnonymusVariableTerm _)
    {
        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(ArithmeticOperationTerm term)
    {
        var leftMaybe = term.Left.Accept(this);
        if(!leftMaybe.HasValue) { return new None<ISimpleTerm>(); }

        var rightMaybe = term.Right.Accept(this);
        if (!rightMaybe.HasValue) { return new None<ISimpleTerm>(); }

        var functor = _operatorConverter.Convert(term.Operation);

        var structure = new Structure(functor, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()], false);

        return new Some<ISimpleTerm>(structure);
    }

    public override IOption<ISimpleTerm> Visit(BasicTerm term)
    {
        var newChildrenMaybes = term.Terms.Select((term) => term.Accept(this));
        if(newChildrenMaybes.Any((maybe) => !maybe.HasValue)) 
        {
            return new None<ISimpleTerm>();
        }

        return new Some<ISimpleTerm>(new Structure(term.Identifier, newChildrenMaybes.Select((m)=> m.GetValueOrThrow()), false));
    }

    public override IOption<ISimpleTerm> Visit(ConventionalList term)
    {
        return new Some<ISimpleTerm>(ConvertConventionalList(term.Terms));
    }

    public override IOption<ISimpleTerm> Visit(NegatedTerm term)
    {
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

        return new None<ISimpleTerm>();
    }

    public override IOption<ISimpleTerm> Visit(NumberTerm term)
    {
        return new Some<ISimpleTerm>(new Integer(term.Value));
    }

    public override IOption<ISimpleTerm> Visit(ParenthesizedTerm term)
    {
        var inner = term.Term.Accept(this);
        try
        {
            return new Some<ISimpleTerm>(new Structure(_functorTable.Parenthesis, [inner.GetValueOrThrow()], false));
        }
        catch
        {
            return new None<ISimpleTerm>();
        }
    }

    public override IOption<ISimpleTerm> Visit(RecursiveList term)
    {
        var convertedList = ConvertRecursiveList(term);

        return new Some<ISimpleTerm>(convertedList);
    }

    public override IOption<ISimpleTerm> Visit(StringTerm term)
    {
        return new Some<ISimpleTerm>(ConvertString(term.Value));
    }

    public override IOption<ISimpleTerm> Visit(VariableTerm term)
    {
        return new Some<ISimpleTerm>(new Variable(term.Identifier));
    }

    private ISimpleTerm ConvertString(string str)
    {
        if (str.Equals(string.Empty))
        {
            return new Structure(_functorTable.Nil, [], false);
        }

        var head = new Structure(str.First().ToString(), Enumerable.Empty<ISimpleTerm>(), false);
        var tail = ConvertString(new string(str.Skip(1).ToArray()));

        return new Structure(_functorTable.List, [head, tail], false);
    }

    private ISimpleTerm ConvertRecursiveList(RecursiveList term)
    {
        var leftMaybe = term.Head.Accept(this);
        if (!leftMaybe.HasValue) throw new ArgumentException(nameof(term));

        var rightMaybe = term.Tail.Accept(this);
        if (!rightMaybe.HasValue) throw new ArgumentException(nameof(term));

        return new Structure(_functorTable.List, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()], false);
    }

    private ISimpleTerm ConvertConventionalList(IEnumerable<ITerm> terms)
    {
        if (terms.Count() == 0) { return new Structure(_functorTable.Nil, [], false); }

        var headMaybe = terms.ElementAt(0).Accept(this);
        if (!headMaybe.HasValue) { throw new ArgumentException(nameof(terms)); }

        return new Structure(_functorTable.List, [headMaybe.GetValueOrThrow() ,ConvertConventionalList(terms.Skip(1))], false);
    }
}
