using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals.ArithmeticEvaluation;

public class ArithmeticEvaluator : ISimpleTermVisitor<IOption<int>>
{
    public IOption<int> Evaluate(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<int> Visit(Division term)
    {
        var leftMaybe = term.Left.Accept(this);
        int left;
        try
        {
            left = leftMaybe.GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }  

        var rightMaybe = term.Right.Accept(this);
        int right;
        try
        {
            right = rightMaybe.GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }

        if (right == 0)
        {
            return new None<int>();
        }

        return new Some<int>(left / right);
    }

    public IOption<int> Visit(Multiplication term)
    {
        var leftMaybe = term.Left.Accept(this);
        int left;
        try
        {
            left = leftMaybe.GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }

        var rightMaybe = term.Right.Accept(this);
        int right;
        try
        {
            right = rightMaybe.GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }

        return new Some<int>(left * right);
    }

    public IOption<int> Visit(Addition term)
    {
        var leftMaybe = term.Left.Accept(this);
        int left;
        try
        {
            left = leftMaybe.GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }

        var rightMaybe = term.Right.Accept(this);
        int right;
        try
        {
            right = rightMaybe.GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }

        return new Some<int>(left + right);
    }

    public IOption<int> Visit(Subtraction term)
    {
        var leftMaybe = term.Left.Accept(this);
        int left;
        try
        {
            left = leftMaybe.GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }

        var rightMaybe = term.Right.Accept(this);
        int right;
        try
        {
            right = rightMaybe.GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }

        return new Some<int>(left - right);
    }

    public IOption<int> Visit(Parenthesis term)
    {
        return term.Term.Accept(this);
    }

    public IOption<int> Visit(Integer integer)
    {
        return new Some<int>(integer.Value);
    }

    // automatic failure cases
    public IOption<int> Visit(Structure _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(Variable _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(GreaterThan _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(GreaterThanOrEqual _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(LessOrEqualThan _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(LessThan _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(List _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(Evaluation _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(Nil _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(ClassicalNegation _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(Naf _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(ForAll _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(DisunificationStructure _)
    {
        return new None<int>();
    }

    public IOption<int> Visit(UnificationStructure _)
    {
        return new None<int>();
    }
}
