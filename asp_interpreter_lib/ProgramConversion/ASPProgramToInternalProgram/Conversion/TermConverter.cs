using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

public class TermConverter : TypeBaseVisitor<ISimpleTerm>
{
    private readonly FunctorTableRecord _functorTable;

    private readonly OperatorConverter _operatorConverter;

    private readonly NegatedTermConverter _negatedTermConverter;

    private int _nextAnonymousVariableIndex = 0;

    public TermConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable, nameof(functorTable));

        _functorTable = functorTable;
        _operatorConverter = new OperatorConverter(functorTable);
        _negatedTermConverter = new NegatedTermConverter(this, functorTable);
    }

    public ISimpleTerm Convert(ITerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this).GetValueOrThrow();
    }

    public override IOption<ISimpleTerm> Visit(AnonymousVariableTerm _)
    {
        ArgumentNullException.ThrowIfNull(_);


        var variable = new Variable($"{_functorTable.AnonymusVariable}{_nextAnonymousVariableIndex}");
        _nextAnonymousVariableIndex += 1;

        return new Some<ISimpleTerm>(variable);
    }

    public override IOption<ISimpleTerm> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var leftMaybe = term.Left.Accept(this);

        var rightMaybe = term.Right.Accept(this);

        var functor = _operatorConverter.Convert(term.Operation);

        var children = new ISimpleTerm[] { leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow() };

        var structure = new Structure(functor, children);

        return new Some<ISimpleTerm>(structure);
    }

    public override IOption<ISimpleTerm> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var newChildrenMaybes = term.Terms.Select((term) => term.Accept(this));
        if(newChildrenMaybes.Any((maybe) => !maybe.HasValue)) 
        {
            return new None<ISimpleTerm>();
        }

        var newChildren = newChildrenMaybes.Select(x => x.GetValueOrThrow()).ToArray();

        return new Some<ISimpleTerm>(new Structure(term.Identifier, newChildren));
    }

    public override IOption<ISimpleTerm> Visit(ConventionalList term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(ConvertConventionalList(term.Terms));
    }

    public override IOption<ISimpleTerm> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var convertedTermMaybe = _negatedTermConverter.Convert(term);

        return convertedTermMaybe;
    }

    public override IOption<ISimpleTerm> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(new Integer(term.Value));
    }

    public override IOption<ISimpleTerm> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var inner = term.Term.Accept(this);
        try
        {
            return new Some<ISimpleTerm>(new Structure(_functorTable.Parenthesis, [inner.GetValueOrThrow()]));
        }
        catch
        {
            return new None<ISimpleTerm>();
        }
    }

    public override IOption<ISimpleTerm> Visit(RecursiveList term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var convertedList = ConvertRecursiveList(term);

        return new Some<ISimpleTerm>(convertedList);
    }

    public override IOption<ISimpleTerm> Visit(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(ConvertString(term.Value));
    }

    public override IOption<ISimpleTerm> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(new Variable(term.Identifier));
    }

    private Structure ConvertString(string str)
    {
        if (str.Equals(string.Empty))
        {
            return new Structure(_functorTable.Nil, []);
        }

        var head = new Structure(str.First().ToString(), []);
        var tail = ConvertString(new string(str.Skip(1).ToArray()));

        return new Structure(_functorTable.List, [head, tail]);
    }

    private Structure ConvertRecursiveList(RecursiveList term)
    {
        var leftMaybe = term.Head.Accept(this);

        var rightMaybe = term.Tail.Accept(this);

        return new Structure(_functorTable.List, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);
    }

    private Structure ConvertConventionalList(IEnumerable<ITerm> terms)
    {
        if (!terms.Any())
        {
            return new Structure(_functorTable.Nil, []); 
        }

        var headMaybe = terms.ElementAt(0).Accept(this);

        return new Structure(_functorTable.List, [headMaybe.GetValueOrThrow() ,ConvertConventionalList(terms.Skip(1))]);
    }
}
