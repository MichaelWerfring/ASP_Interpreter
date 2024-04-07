using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class TermConverter : TypeBaseVisitor<IInternalTerm>
{
    public IInternalTerm Convert(ITerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var result = term.Accept(this);
        IInternalTerm convertedTerm;
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

    public override IOption<IInternalTerm> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<IInternalTerm>(new Variable(term.Identifier));
    }

    public override IOption<IInternalTerm> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var newChildren = new IInternalTerm[term.Terms.Count];
        for (int i = 0; i < term.Terms.Count; i++)
        {
            var result = term.Terms[i].Accept(this);
            try
            {
                newChildren[i] = result.GetValueOrThrow();
            }
            catch
            {
                return new None<IInternalTerm>();
            }
        }

        return new Some<IInternalTerm>(new Structure(term.Identifier, newChildren));
    }

    public override IOption<IInternalTerm> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<IInternalTerm>(new Integer(term.Value));
    }

    public override IOption<IInternalTerm> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var innerTermResult = term.Term.Accept(this);
        IInternalTerm innerTerm;
        try
        {
            innerTerm = innerTermResult.GetValueOrThrow();
        }
        catch
        {
            return new None<IInternalTerm>();
        }

        return new Some<IInternalTerm>(new Structure("NEG", new List<IInternalTerm>() { innerTerm }));
    }
}
