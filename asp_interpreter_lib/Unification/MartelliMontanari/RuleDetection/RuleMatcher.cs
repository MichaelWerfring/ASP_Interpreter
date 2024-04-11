using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Unification.MartelliMontanariUnificationAlgorithm.RuleDetection.Rules;

namespace asp_interpreter_lib.Unification.MantelliMontanari.RuleDetection;

public class RuleMatcher
{
    private SimpleTermComparer _termEquivalenceChecker;
    private SimpleTermContainsChecker _termContainsChecker;

    private bool _doOccursCheck;

    public RuleMatcher(bool doOccursCheck)
    {
        _termEquivalenceChecker = new SimpleTermComparer();
        _termContainsChecker = new SimpleTermContainsChecker();

        _doOccursCheck = doOccursCheck;
    }

    /// <summary>
    /// Find out which case matches the equation, and get appropriate rule (if exists).
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when equation is not in equations.</exception>
    public IOption<IMMRule> GetAppropriateRule((ISimpleTerm, ISimpleTerm) equation, IEnumerable<(ISimpleTerm, ISimpleTerm)> equations)
    {
        ArgumentNullException.ThrowIfNull(equation);
        ArgumentNullException.ThrowIfNull(equations);
        if (!equations.Contains(equation))
        {
            throw new ArgumentException(nameof(equations), $"Must contain {nameof(equation)}.");
        }

        if (IsRewriteCase(equation))
        {
            return new Some<IMMRule>(new RewriteRule());
        }

        if (IsErasureCase(equation))
        {
            return new Some<IMMRule>(new ErasureRule());
        }

        if (IsReductionCase(equation))
        {
            return new Some<IMMRule>(new ReductionRule());
        }

        if (IsReductionFailureCase(equation))
        {
            return new Some<IMMRule>(new FailureRule());
        }

        if (IsEliminationCase(equation, equations))
        {
            return new Some<IMMRule>(new EliminationRule());
        }

        if (_doOccursCheck && IsOccursCheckFailureCase(equation))
        {
            return new Some<IMMRule>(new FailureRule());
        }

        return new None<IMMRule>();
    }

    /// <summary>
    /// It is a rewrite case if left is not a variable and right is a variable.
    /// </summary>
    private bool IsRewriteCase((ISimpleTerm, ISimpleTerm) equation)
    {
        if (equation.Item1 is Structure && equation.Item2 is Variable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// It is an erasure case if both are variables and they are equal.
    /// </summary>
    private bool IsErasureCase((ISimpleTerm, ISimpleTerm) equation)
    {
        if
        (
            equation.Item1 is Variable 
            &&
            equation.Item1 is Variable
            &&
            _termEquivalenceChecker.Equals(equation.Item1, equation.Item2)
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// It is a reduction case if both are structures with same functor, negation status and children count.
    /// </summary>
    private bool IsReductionCase((ISimpleTerm, ISimpleTerm) equation)
    {
        if (!(equation.Item1 is Structure && equation.Item2 is Structure))
        {
            return false;
        }

        var left = equation.Item1 as Structure;
        var right = equation.Item2 as Structure;

        if (left!.Functor != right!.Functor) { return false; }

        if (left.Children.Count() != right.Children.Count()) { return false; };

        return true;
    }

    /// <summary>
    /// It is a reduction failure case if both are structures, with functor or children count not being equal.
    /// </summary>
    private bool IsReductionFailureCase((ISimpleTerm, ISimpleTerm) equation)
    {
        if (!(equation.Item1 is Structure && equation.Item2 is Structure))
        {
            return false;
        }

        var left = equation.Item1 as Structure;
        var right = equation.Item2 as Structure;

        return left!.Functor != right!.Functor || left.Children.Count() != right.Children.Count();
    }

    /// <summary>
    /// It is an elimination case if:
    /// left is a variable,
    /// right is not equal to left,
    /// right does not contain left,
    /// left is found somewhere else in the set of equations.
    /// </summary>
    private bool IsEliminationCase((ISimpleTerm, ISimpleTerm) equation, IEnumerable<(ISimpleTerm, ISimpleTerm)> equations)
    {
        if (equation.Item1 is Structure)
        {
            return false;
        }

        if (_termEquivalenceChecker.Equals(equation.Item1, equation.Item2))
        {
            return false;
        }

        if (_termContainsChecker.LeftContainsRight(equation.Item2, equation.Item1))
        {
            return false;
        }

        IEnumerable<(ISimpleTerm, ISimpleTerm)> filteredList = equations.ToList();
        filteredList = filteredList.Where((eq) =>
        {
            return eq != equation;
        })
            .Where((eq) =>
        {
            return _termContainsChecker.LeftContainsRight(eq.Item1, equation.Item1)
            ||
            _termContainsChecker.LeftContainsRight(eq.Item2, equation.Item1);
        });
        if (filteredList.Count() == 0)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// It is an occurs check failure case if left is a variable, right is not equal to left, and right contains left.
    private bool IsOccursCheckFailureCase((ISimpleTerm, ISimpleTerm) equation)
    {
        if (equation.Item1 is Structure)
        {
            return false;
        }

        if (_termEquivalenceChecker.Equals(equation.Item1, equation.Item2))
        {
            return false;
        }

        if (!_termContainsChecker.LeftContainsRight(equation.Item2, equation.Item1))
        {
            return false;
        }

        return true;
    }
}
