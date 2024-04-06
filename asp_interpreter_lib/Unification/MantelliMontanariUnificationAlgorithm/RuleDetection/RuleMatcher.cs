using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection.Rules;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection;

public class RuleMatcher
{
    private InternalTermComparer _termEquivalenceChecker;
    private InternalTermContainsChecker _termContainsChecker;
    private VariabilityDecider _variabilityDecider;
    private ReducabilityDecider _collapsabilityDecider;

    private bool _doOccursCheck;

    public RuleMatcher(bool doOccursCheck)
    {
        _termEquivalenceChecker = new InternalTermComparer();
        _termContainsChecker = new InternalTermContainsChecker();
        _variabilityDecider = new VariabilityDecider();
        _collapsabilityDecider = new ReducabilityDecider();

        _doOccursCheck = doOccursCheck;
    }

    /// <summary>
    /// Find out which case matches the equation, and get appropriate rule (if exists).
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when equation is not in equations.</exception>
    public IOption<IMMRule> GetAppropriateRule((IInternalTerm, IInternalTerm) equation, IEnumerable<(IInternalTerm, IInternalTerm)> equations)
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
    /// It is an erasure case if left is not a variable and right is a variable.
    /// </summary>
    private bool IsRewriteCase((IInternalTerm, IInternalTerm) equation)
    {
        if (!_variabilityDecider.CountsAsVariable(equation.Item1) && _variabilityDecider.CountsAsVariable(equation.Item2))
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
    private bool IsErasureCase((IInternalTerm, IInternalTerm) equation)
    {
        if
        (
            _variabilityDecider.CountsAsVariable(equation.Item1) 
            && 
            _variabilityDecider.CountsAsVariable(equation.Item2)
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
    private bool IsReductionCase((IInternalTerm, IInternalTerm) equation)
    {
        return _collapsabilityDecider.CanReduce(equation.Item1, equation.Item2);
    }

    /// <summary>
    /// It is a reduction failure case if both are structures, with negation status or functor or children count not being equal.
    /// </summary>
    private bool IsReductionFailureCase((IInternalTerm, IInternalTerm) equation)
    {
        if 
        (
            _variabilityDecider.CountsAsVariable(equation.Item1)
            ||
            _variabilityDecider.CountsAsVariable(equation.Item2)
        )
        {
            return false;
        }

        return !_collapsabilityDecider.CanReduce(equation.Item1, equation.Item2);
    }

    /// <summary>
    /// It is an elimination case if:
    /// left is a variable,
    /// right is not equal to left,
    /// right does not contain left,
    /// left is found somewhere else in the set of equations.
    /// </summary>
    private bool IsEliminationCase((IInternalTerm, IInternalTerm) equation, IEnumerable<(IInternalTerm, IInternalTerm)> equations)
    {
        if (!_variabilityDecider.CountsAsVariable(equation.Item1))
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

        IEnumerable<(IInternalTerm, IInternalTerm)> filteredList = equations.ToList();
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
    private bool IsOccursCheckFailureCase((IInternalTerm, IInternalTerm) equation)
    {
        if (!_variabilityDecider.CountsAsVariable(equation.Item1))
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
