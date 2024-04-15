using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Reducer;

public class StructureReducer : ISimpleTermArgsVisitor<IOption<TermReduction>, ISimpleTerm>
{
    public IOption<TermReduction> Reduce(IStructure left, IStructure right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left.Accept(this, right);
    }

    public IOption<TermReduction> Visit(Variable left, ISimpleTerm right)
    {
        return new None<TermReduction>();
    }

    public IOption<TermReduction> Visit(Structure left, ISimpleTerm right)
    {
        Structure other;
        try
        {
            other = (Structure)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        if (left.Functor != other.Functor)
        {
            return new None<TermReduction>();
        }

        if (left.Children.Count() != other.Children.Count())
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction(left.Children, other.Children));

    }

    public IOption<TermReduction> Visit(Division left, ISimpleTerm right)
    {
        Division other;
        try
        {
            other = (Division)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(Multiplication left, ISimpleTerm right)
    {
        Multiplication other;
        try
        {
            other = (Multiplication)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(Addition left, ISimpleTerm right)
    {
        Addition other;
        try
        {
            other = (Addition)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(Subtraction left, ISimpleTerm right)
    {
        Subtraction other;
        try
        {
            other = (Subtraction)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(GreaterThan left, ISimpleTerm right)
    {
        GreaterThan other;
        try
        {
            other = (GreaterThan)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(GreaterThanOrEqual left, ISimpleTerm right)
    {
        GreaterThanOrEqual other;
        try
        {
            other = (GreaterThanOrEqual)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(LessOrEqualThan left, ISimpleTerm right)
    {
        LessOrEqualThan other;
        try
        {
            other = (LessOrEqualThan)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(LessThan left, ISimpleTerm right)
    {
        LessThan other;
        try
        {
            other = (LessThan)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(List left, ISimpleTerm right)
    {
        List other;
        try
        {
            other = (List)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(Parenthesis left, ISimpleTerm right)
    {
        Parenthesis other;
        try
        {
            other = (Parenthesis)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Term], [other.Term]));
    }

    public IOption<TermReduction> Visit(Evaluation left, ISimpleTerm right)
    {
        Evaluation other;
        try
        {
            other = (Evaluation)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(Integer left, ISimpleTerm right)
    {
        Integer other;
        try
        {
            other = (Integer)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        if (left.Value != other.Value)
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([], []));
    }

    public IOption<TermReduction> Visit(Nil left, ISimpleTerm right)
    {
        Nil other;
        try
        {
            other = (Nil)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([], []));
    }

    public IOption<TermReduction> Visit(ClassicalNegation left, ISimpleTerm right)
    {
        ClassicalNegation other;
        try
        {
            other = (ClassicalNegation)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Term], [other.Term]));
    }

    public IOption<TermReduction> Visit(Naf left, ISimpleTerm right)
    {
        Naf other;
        try
        {
            other = (Naf)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Term], [other.Term]));
    }

    public IOption<TermReduction> Visit(ForAll left, ISimpleTerm right)
    {
        ForAll other;
        try
        {
            other = (ForAll)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.VarTerm, left.Goal], [other.VarTerm, other.Goal]));
    }

    public IOption<TermReduction> Visit(DisunificationStructure left, ISimpleTerm right)
    {
        DisunificationStructure other;
        try
        {
            other = (DisunificationStructure)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }

    public IOption<TermReduction> Visit(UnificationStructure left, ISimpleTerm right)
    {
        UnificationStructure other;
        try
        {
            other = (UnificationStructure)right;
        }
        catch
        {
            return new None<TermReduction>();
        }

        return new Some<TermReduction>(new TermReduction([left.Left, left.Right], [other.Left, other.Right]));
    }
}
