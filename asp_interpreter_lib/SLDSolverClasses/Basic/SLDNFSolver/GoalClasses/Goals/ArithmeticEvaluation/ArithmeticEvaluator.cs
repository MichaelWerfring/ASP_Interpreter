using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.ArithmeticEvaluation;

public class ArithmeticEvaluator : ISimpleTermVisitor<IOption<int>>
{
    private FunctorTableRecord _functorTable;

    public ArithmeticEvaluator(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        _functorTable = functorTable;
    }

    public IOption<int> Evaluate(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<int> Visit(Integer integer)
    {
        return new Some<int>(integer.Value);
    }

    public IOption<int> Visit(Structure structure)
    {
        if (structure.Children.Count() != 2) return new None<int>();

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

        if (structure.Functor == _functorTable.Addition)
        {
            return new Some<int>(leftVal + rightVal);
        }
        else if (structure.Functor == _functorTable.Multiplication)
        {
            return new Some<int>(leftVal * rightVal);
        }
        else if (structure.Functor == _functorTable.Division && rightVal != 0)
        {
            return new Some<int>(leftVal / rightVal);
        }
        else if (structure.Functor == _functorTable.Subtraction)
        {
            return new Some<int>(leftVal - rightVal);
        }
        else if (structure.Functor == _functorTable.Power)
        {
            return new Some<int>(Convert.ToInt32(Math.Pow(leftVal, rightVal)));
        }

        return new None<int>();
    }

    public IOption<int> Visit(Variable _)
    {
        return new None<int>();
    }
}
