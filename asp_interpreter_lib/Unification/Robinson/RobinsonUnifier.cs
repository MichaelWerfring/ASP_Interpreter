using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;
using asp_interpreter_lib.SLDSolverClasses.StandardSolver.VariableRenamer;
using asp_interpreter_lib.Unification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.Robinson;

public class RobinsonUnifier : ISimpleTermArgsVisitor<ISimpleTerm>
{
    private SimpleTermComparer _comparer = new SimpleTermComparer();
    private VariableSubstituter _substituter = new VariableSubstituter();
    private SimpleTermContainsChecker _containsChecker = new SimpleTermContainsChecker();

    private bool _doOccursCheck;

    private Dictionary<Variable, ISimpleTerm> _substitution;
    private bool _hasSucceded;

    public RobinsonUnifier(bool doOccursCheck)
    {
        _doOccursCheck = doOccursCheck;

        _substitution = new Dictionary<Variable, ISimpleTerm>(new VariableComparer());
        _hasSucceded = true;
    }

    public IOption<Dictionary<Variable, ISimpleTerm>> Unify(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        TryUnify(left, right);

        if (!_hasSucceded)
        {
            return new None<Dictionary<Variable, ISimpleTerm>>();
        }

        return new Some<Dictionary<Variable, ISimpleTerm>>(_substitution);
    }

    public void Visit(Variable left, ISimpleTerm right)
    {
        // if both are variables and are equal
        if (_comparer.Equals(left, right))
        {
            return;
        }
        // now we know that left is variable : do occurs check if necessary
        else if (_doOccursCheck && _containsChecker.LeftContainsRight(right, left))
        {
            _hasSucceded = false;
            return;
        }
        // now do substitution composition
        else
        {
            var newDict = new Dictionary<Variable, ISimpleTerm>(new VariableComparer())
            {
                { left, right }
            };
            _substitution = ApplySubstitutionComposition(_substitution, newDict);
        }
    }

    public void Visit(Structure left, ISimpleTerm right)
    {
        // if both are structures
        if (right is Structure rightStruct)
        {
            if (left.Functor != rightStruct.Functor)
            {
                _hasSucceded = false;
                return;
            }

            if (left.Children.Count() != rightStruct.Children.Count())
            {
                _hasSucceded = false;
                return;
            }

            for (int i = 0; i < left.Children.Count(); i++)
            {
                TryUnify(left.Children.ElementAt(i), rightStruct.Children.ElementAt(i));
            }
        }
        // left is structure and right variable
        else
        {
            TryUnify(right, left);
        }
    }

    private void TryUnify
    (
        ISimpleTerm left,
        ISimpleTerm right
    )
    {
        if (!_hasSucceded) { return; }

        ISimpleTerm currentLeft = left;
        ISimpleTerm currentRight = right;
        if (left is Variable leftVar)
        {
            currentLeft = GetSubstitutionOrDefault(leftVar, _substitution);
        }
        if (right is Variable rightVar)
        {
            currentRight = GetSubstitutionOrDefault(rightVar, _substitution);
        }

        currentLeft.Accept(this, currentRight);
    }

    private Dictionary<Variable, ISimpleTerm> ApplySubstitutionComposition
    (
        Dictionary<Variable, ISimpleTerm> oldSubstitution,
        Dictionary<Variable, ISimpleTerm> substitution
    )
    {
        return oldSubstitution.Select(keyValuePair => (keyValuePair.Key, _substituter.Substitute(keyValuePair.Value, substitution)))
                              .ToDictionary(new VariableComparer())
                              .Union(substitution)
                              .ToDictionary(new VariableComparer());
    }

    private ISimpleTerm GetSubstitutionOrDefault(Variable variable, Dictionary<Variable, ISimpleTerm> substitution)
    {
        ISimpleTerm? potentialSub;

        substitution.TryGetValue(variable, out potentialSub);

        if (potentialSub == null)
        {
            return variable;
        }

        return potentialSub;
    }
}
