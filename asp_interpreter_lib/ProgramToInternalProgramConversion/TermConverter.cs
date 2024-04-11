using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class TermConverter : TypeBaseVisitor<ISimpleTerm>
{
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

    public override IOption<ISimpleTerm> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(new Variable(term.Identifier));
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

    public override IOption<ISimpleTerm> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(new Structure(term.Value.ToString(), Enumerable.Empty<ISimpleTerm>()));
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

        return new Some<ISimpleTerm>(new Structure("NEG", new List<ISimpleTerm>() { innerTerm }));
    }
}
