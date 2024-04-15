using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.ArithmeticOperations;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermCloner : ISimpleTermVisitor<ISimpleTerm>
{
    public ISimpleTerm Clone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public ISimpleTerm Visit(Variable term)
    {
        return new Variable(term.Identifier);
    }

    public ISimpleTerm Visit(Structure term)
    {
        var copiedChildren = new List<ISimpleTerm>();
        foreach (var child in term.Children)
        {
            copiedChildren.Add(child.Accept(this));
        }

        return new Structure(term.Functor.ToString(), copiedChildren);
    }

    public ISimpleTerm Visit(Division term)
    {
        return new Division(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(Multiplication term)
    {
        return new Multiplication(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(Addition term)
    {
        return new Addition(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(Subtraction term)
    {
        return new Subtraction(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(GreaterThan term)
    {
        return new GreaterThan(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(GreaterThanOrEqual term)
    {
        return new GreaterThanOrEqual(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(LessOrEqualThan term)
    {
        return new LessOrEqualThan(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(LessThan term)
    {
        return new LessThan(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(List term)
    {
        return new List(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(Parenthesis term)
    {
        return new Parenthesis(term.Term.Accept(this));
    }

    public ISimpleTerm Visit(Evaluation term)
    {
        return new Evaluation(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(Integer term)
    {
        return new Integer(term.Value);
    }

    public ISimpleTerm Visit(Nil term)
    {
        return new Nil();
    }

    public ISimpleTerm Visit(ClassicalNegation term)
    {
        return new ClassicalNegation(term.Term.Accept(this));
    }

    public ISimpleTerm Visit(Naf term)
    {
        return new Naf(term.Term.Accept(this));
    }

    public ISimpleTerm Visit(ForAll term)
    {
        return new ForAll(term.VarTerm.Accept(this), term.Goal.Accept(this));
    }

    public ISimpleTerm Visit(DisunificationStructure term)
    {
        return new DisunificationStructure(term.Left.Accept(this), term.Right.Accept(this));
    }

    public ISimpleTerm Visit(UnificationStructure term)
    {
        return new UnificationStructure(term.Left.Accept(this), term.Right.Accept(this));
    }
}
