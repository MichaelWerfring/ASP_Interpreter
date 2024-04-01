using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.SimplifiedTerm;
using asp_interpreter_lib.SimplifiedTerm.TermFunctionality;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection.Rules;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection;

public class RuleMatcher
{
    private TermEquivalenceChecker _termEquivalenceChecker;
    private TermContainsChecker _termContainsChecker;
    private bool _doOccursCheck;

    public RuleMatcher(bool doOccursCheck)
    {
        _termEquivalenceChecker = new TermEquivalenceChecker();
        _termContainsChecker = new TermContainsChecker();

        _doOccursCheck = doOccursCheck;
    }

    /// <summary>
    /// Find out which case matches the equation, and get appropriate rule (if exists).
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when equation is not in equations.</exception>
    public IOption<IMMRule> GetAppropriateRule((ISimplifiedTerm, ISimplifiedTerm) equation, IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)> equations)
    {
        ArgumentNullException.ThrowIfNull(equation);
        ArgumentNullException.ThrowIfNull(equations);
        if(! equations.Contains(equation))
        {
            throw new ArgumentException(nameof(equations), $"Must contain {nameof(equation)}.");
        }

        if(IsRewriteCase(equation))
        {
            return new Some<IMMRule>(new RewriteRule());
        }

        if(IsErasureCase(equation))
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
    private bool IsRewriteCase( (ISimplifiedTerm, ISimplifiedTerm) equation)
    {     
        if(equation.Item1.GetType() != typeof(VariableTerm) && equation.Item2.GetType() == typeof(VariableTerm))
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
    private bool IsErasureCase( (ISimplifiedTerm, ISimplifiedTerm) equation)
    {
        if 
        (
            equation.Item1.GetType() == typeof(VariableTerm)
            && 
            equation.Item2.GetType() == typeof(VariableTerm)
            && 
            _termEquivalenceChecker.AreEqual(equation.Item1, equation.Item2)
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
    /// It is a reduction case if both are basic terms with same functor, negation status and children count.
    /// </summary>
    private bool IsReductionCase((ISimplifiedTerm, ISimplifiedTerm) equation)
    {
        if (!(equation.Item1.GetType() == typeof(BasicTerm) && equation.Item2.GetType() == typeof(BasicTerm)))
        {
            return false;
        }

        BasicTerm aVar = (BasicTerm)equation.Item1;
        BasicTerm bVar = (BasicTerm)equation.Item2;

        if (aVar.Functor != bVar.Functor)
        {
            return false;
        }

        if (aVar.IsNegated != bVar.IsNegated)
        {
            return false;
        }

        if (aVar.Children.Count() != bVar.Children.Count())
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// It is a reduction failure case if both are basic terms, with negation status or functor or children count not being equal.
    /// </summary>
    private bool IsReductionFailureCase((ISimplifiedTerm, ISimplifiedTerm) equation)
    {
        if (!(equation.Item1.GetType() == typeof(BasicTerm) && equation.Item2.GetType() == typeof(BasicTerm)))
        {
            return false;
        }
        BasicTerm aVar = (BasicTerm)equation.Item1;
        BasicTerm bVar = (BasicTerm)equation.Item2;

        if (aVar.IsNegated != bVar.IsNegated || aVar.Functor != bVar.Functor || aVar.Children.Count() != bVar.Children.Count())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// It is an elimination case if:
    /// left is a variable,
    /// right is not equal to left,
    /// right does not contain left,
    /// left is found somewhere else in the set of equations.
    /// </summary>
    private bool IsEliminationCase((ISimplifiedTerm, ISimplifiedTerm) equation, IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)> equations)
    {
        if (equation.Item1.GetType() != typeof(VariableTerm))
        {
            return false;
        }

        if (_termEquivalenceChecker.AreEqual(equation.Item1,equation.Item2))
        {
            return false;
        }

        if (_termContainsChecker.LeftContainsRight(equation.Item2, equation.Item1))
        {
            return false;
        }

        IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)> filteredList = equations.ToList();
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
        if(filteredList.Count() == 0)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// It is an occurs check failure case if left is a variable, right is not equal to left, and right contains left.
    private bool IsOccursCheckFailureCase((ISimplifiedTerm, ISimplifiedTerm) equation)
    {
        if (equation.Item1.GetType() != typeof (VariableTerm))
        { 
            return false; 
        }

        if (_termEquivalenceChecker.AreEqual(equation.Item1, equation.Item2))
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
