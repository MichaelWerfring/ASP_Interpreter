using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

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
        var variable = new Variable($"{_functorTable.AnonymusVariable}{_nextAnonymousVariableIndex}");
        _nextAnonymousVariableIndex+=1;

        return new Some<ISimpleTerm>(variable);
    }

    public override IOption<ISimpleTerm> Visit(ArithmeticOperationTerm term)
    {
        var leftMaybe = term.Left.Accept(this);
        if(!leftMaybe.HasValue) { return new None<ISimpleTerm>(); }

        var rightMaybe = term.Right.Accept(this);
        if (!rightMaybe.HasValue) { return new None<ISimpleTerm>(); }

        var functor = _operatorConverter.Convert(term.Operation);

        var structure = new Structure(functor, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);

        return new Some<ISimpleTerm>(structure);
    }

    public override IOption<ISimpleTerm> Visit(BasicTerm term)
    {
        var newChildrenMaybes = term.Terms.Select((term) => term.Accept(this));
        if(newChildrenMaybes.Any((maybe) => !maybe.HasValue)) 
        {
            return new None<ISimpleTerm>();
        }

        return new Some<ISimpleTerm>(new Structure(term.Identifier, newChildrenMaybes.Select((m)=> m.GetValueOrThrow()).ToImmutableList()));
    }

    public override IOption<ISimpleTerm> Visit(ConventionalList term)
    {
        return new Some<ISimpleTerm>(ConvertConventionalList(term.Terms));
    }

    public override IOption<ISimpleTerm> Visit(NegatedTerm term)
    {
        var convertedTerm = _negatedTermConverter.Convert(term);

        return new Some<ISimpleTerm>(convertedTerm);
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
            return new Some<ISimpleTerm>(new Structure(_functorTable.Parenthesis, [inner.GetValueOrThrow()]));
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
            return new Structure(_functorTable.Nil, []);
        }

        var head = new Structure(str.First().ToString(), ImmutableList.Create<ISimpleTerm>());
        var tail = ConvertString(new string(str.Skip(1).ToArray()));

        return new Structure(_functorTable.List, [head, tail]);
    }

    private ISimpleTerm ConvertRecursiveList(RecursiveList term)
    {
        var leftMaybe = term.Head.Accept(this);
        if (!leftMaybe.HasValue) throw new ArgumentException(nameof(term));

        var rightMaybe = term.Tail.Accept(this);
        if (!rightMaybe.HasValue) throw new ArgumentException(nameof(term));

        return new Structure(_functorTable.List, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);
    }

    private ISimpleTerm ConvertConventionalList(IEnumerable<ITerm> terms)
    {
        if (terms.Count() == 0) { return new Structure(_functorTable.Nil, []); }

        var headMaybe = terms.ElementAt(0).Accept(this);
        if (!headMaybe.HasValue) { throw new ArgumentException(nameof(terms)); }

        return new Structure(_functorTable.List, [headMaybe.GetValueOrThrow() ,ConvertConventionalList(terms.Skip(1))]);
    }
}
