using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermContainsChecker : ISimpleTermArgsVisitor<bool, ISimpleTerm>
{
    private SimpleTermComparer _equivalenceChecker = new SimpleTermComparer();

    public bool LeftContainsRight(ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.Accept(this, other);
    }

    public bool Visit(Variable term, ISimpleTerm other)
    {
        return _equivalenceChecker.Visit(term, other);
    }

    public bool Visit(Structure term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        foreach (var child in term.Children)
        {
            bool containsEqualChild = child.Accept(this, other);

            if (containsEqualChild)
            {
                return true;
            }
        }

        return false;
    }

    public bool Visit(Division term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(Multiplication term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(Addition term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(Subtraction term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(GreaterThan term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(GreaterThanOrEqual term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(LessOrEqualThan term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(LessThan term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(List term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(Parenthesis term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Term.Accept(this, other);
    }

    public bool Visit(Evaluation term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(Integer term, ISimpleTerm other)
    {
        return _equivalenceChecker.Visit(term, other);
    }

    public bool Visit(Nil term, ISimpleTerm other)
    {
        return _equivalenceChecker.Visit(term, other);
    }

    public bool Visit(ClassicalNegation term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Term.Accept(this, other);
    }

    public bool Visit(Naf term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Term.Accept(this, other);
    }

    public bool Visit(ForAll term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.VarTerm.Accept(this, other) || term.Goal.Accept(this, other);
    }

    public bool Visit(DisunificationStructure term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }

    public bool Visit(UnificationStructure term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
        if (areEqual)
        {
            return true;
        }

        return term.Left.Accept(this, other) || term.Right.Accept(this, other);
    }
}
