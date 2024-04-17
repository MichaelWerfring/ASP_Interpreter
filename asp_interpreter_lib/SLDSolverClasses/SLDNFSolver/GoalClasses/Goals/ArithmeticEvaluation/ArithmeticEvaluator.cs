using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals.ArithmeticEvaluation;

public class ArithmeticEvaluator : ISimpleTermVisitor<IOption<int>>
{
    public IOption<int> Evaluate(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<int> Visit(Integer integer)
    {
        return new Some<int>(integer.Value);
    }

    // automatic failure cases
    public IOption<int> Visit(Structure structure)
    {
        if(structure.IsNegated) return new Some<int>(0);

        if(structure.Children.Count() != 2) return new None<int>();

        int leftVal;
        int rightVal;
        try
        {
            leftVal = structure.Children.ElementAt(0).Accept(this).GetValueOrThrow();
            rightVal = structure.Children.ElementAt(1).Accept(this).GetValueOrThrow();
        }
        catch
        {
            return new None<int>();
        }
        if(structure.Functor == "+")
        {
            return new Some<int>(leftVal + rightVal);
        }
        else if (structure.Functor == "*")
        {
            return new Some<int>(leftVal * rightVal);
        }
        else if (structure.Functor == "/" && rightVal != 0)
        {
            return new Some<int>(leftVal / rightVal);
        }
        else if (structure.Functor == "-")
        {
            return new Some<int>(leftVal - rightVal);
        }

        return new None<int>();
    }

    public IOption<int> Visit(Variable _)
    {
        return new None<int>();
    }
}
