using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.SLDSolverClasses.VariableRenaming;

public class VariableExtractor : ISimpleTermVisitor<IEnumerable<Variable>>
{
    public HashSet<Variable> GetVariableNames(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this).ToHashSet(new VariableComparer());
    }

    public IEnumerable<Variable> Visit(Variable term)
    {
        return new List<Variable>() { term };
    }

    public IEnumerable<Variable> Visit(Structure term)
    {
        IEnumerable<Variable> result = new List<Variable>();

        foreach (var child in term.Children)
        {
            var childVars = child.Accept(this);

            result = result.Concat(childVars);
        }

        return result;
    }

    public IEnumerable<Variable> Visit(Division term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(Multiplication term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(Addition term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(Subtraction term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(GreaterThan term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(GreaterThanOrEqual term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(LessOrEqualThan term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(LessThan term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(List term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(Parenthesis term)
    {
        return term.Term.Accept(this);
    }

    public IEnumerable<Variable> Visit(Evaluation term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(Integer term)
    {
        return Enumerable.Empty<Variable>();
    }

    public IEnumerable<Variable> Visit(Nil term)
    {
        return Enumerable.Empty<Variable>();
    }

    public IEnumerable<Variable> Visit(ClassicalNegation term)
    {
        return term.Term.Accept(this);
    }

    public IEnumerable<Variable> Visit(Naf term)
    {
        return term.Term.Accept(this);
    }

    public IEnumerable<Variable> Visit(ForAll term)
    {
        var left = term.VarTerm.Accept(this);
        var right = term.Goal.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(DisunificationStructure term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }

    public IEnumerable<Variable> Visit(UnificationStructure term)
    {
        var left = term.Left.Accept(this);
        var right = term.Right.Accept(this);

        return left.Concat(right);
    }
}
