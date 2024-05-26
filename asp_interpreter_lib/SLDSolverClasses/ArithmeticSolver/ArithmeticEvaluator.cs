using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

namespace Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;

public class ArithmeticEvaluator : ISimpleTermVisitor<IOption<int>>
{
    private readonly IImmutableDictionary<string, Func<int, int, IOption<int>>> _evaluationFunctions;

    public ArithmeticEvaluator(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        _evaluationFunctions = new Dictionary<string, Func<int, int, IOption<int>>>()
        {
            {functorTable.Addition, (left, right) => new Some<int>(left + right) },
            {functorTable.Subtraction, (left, right) => new Some<int>(left - right) },
            {functorTable.Multiplication, (left, right) => new Some<int>(left * right) },
            {functorTable.Division, (left, right) =>
                {
                    if (right != 0)
                    {
                        return new Some<int>(left / right);
                    }
                    else
                    {
                        return new None<int>();
                    }
                }
            },
            {functorTable.Power, (left, right) => new Some<int>(Convert.ToInt32(Math.Pow(left, right))) }
        }.ToImmutableDictionary();
    }

    public IOption<int> Evaluate(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<int> Visit(Integer integer)
    {
        ArgumentNullException.ThrowIfNull(integer);

        return new Some<int>(integer.Value);
    }

    public IOption<int> Visit(Structure structure)
    {
        ArgumentNullException.ThrowIfNull(structure);

        if (structure.Children.Count != 2)
        {
            return new None<int>();
        }

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

        _evaluationFunctions.TryGetValue(structure.Functor, out Func<int, int, IOption<int>>? func);

        if (func == null)
        {
            return new None<int>();
        }

        return func.Invoke(leftVal, rightVal);
    }

    public IOption<int> Visit(Variable _)
    {
        ArgumentNullException.ThrowIfNull(_);

        return new None<int>();
    }
}
