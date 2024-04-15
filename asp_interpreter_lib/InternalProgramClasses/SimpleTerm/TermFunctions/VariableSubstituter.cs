using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class VariableSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, Dictionary<Variable, ISimpleTerm>>
{
    private SimpleTermCloner _cloner = new SimpleTermCloner();

    public ISimpleTerm Substitute(ISimpleTerm term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);

        return term.Accept(this, mapping);
    }

    public ISimpleTerm Visit(Variable term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        ISimpleTerm? value;
        mapping.TryGetValue(term, out value);

        if (value == null)
        {
            return new Variable(term.Identifier);
        }

        return _cloner.Clone(value);
    }

    public ISimpleTerm Visit(Structure term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        var newChildren = new ISimpleTerm[term.Children.Count()];

        for (int i = 0; i < newChildren.Length; i++)
        {
            newChildren[i] = term.Children.ElementAt(i).Accept(this, mapping);
        }

        return new Structure(term.Functor.ToString(), newChildren);
    }

    public ISimpleTerm Visit(Division term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Division(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(Multiplication term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Multiplication(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(Addition term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Addition(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(Subtraction term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Subtraction(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(GreaterThan term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new GreaterThan(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(GreaterThanOrEqual term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new GreaterThan(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(LessOrEqualThan term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new LessOrEqualThan(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(LessThan term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new LessThan(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(List term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new List(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(Parenthesis term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Parenthesis(term.Term.Accept(this, mapping));
    }

    public ISimpleTerm Visit(Evaluation term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Evaluation(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(Integer term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Integer(term.Value);
    }

    public ISimpleTerm Visit(Nil term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Nil();
    }

    public ISimpleTerm Visit(ClassicalNegation term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new ClassicalNegation(term.Term.Accept(this, mapping));
    }

    public ISimpleTerm Visit(Naf term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Naf(term.Term.Accept(this, mapping));
    }

    public ISimpleTerm Visit(ForAll term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new ForAll(term.VarTerm.Accept(this, mapping), term.Goal.Accept(this, mapping));
    }

    public ISimpleTerm Visit(DisunificationStructure term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new DisunificationStructure(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }

    public ISimpleTerm Visit(UnificationStructure term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new UnificationStructure(term.Left.Accept(this, mapping), term.Right.Accept(this, mapping));
    }
}
